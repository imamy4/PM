function GetBacklogGrid(projectId) {
    Ext.Loader.setConfig({ enabled: true });
    Ext.Loader.setPath('Ext.ux', '/Scripts/extjs/src/ux/grid/filter');
    Ext.require([
        'Ext.ux.grid.FiltersFeature',
        'Ext.ux.grid.filter.*'
    ]);

    var filters = {
        ftype: 'filters',
        local: true
    };

    var backlogGridPanel = Ext.create('Ext.grid.Panel', {
        stripeRows: true,
        border: false,
        features: [{
            ftype: 'summary'
        }, filters],
        plugins: [{
            ptype: 'rowediting',
            clicksToEdit: 2
        }],
        viewConfig: {
            plugins: {
                ptype: 'gridviewdragdrop',
                dragGroup: 'thisGridDDGroup',
                dropGroup: 'thisGridDDGroup'
            },
            listeners: {// node, какую строку тащу, на место какой ставлю, до или после встает
                drop: function (node, dragRowData, dropRow, dropPosition) {
                    var dragData = dragRowData.records[0].data,
                        dropData = dropRow ? dropRow.data : null;

                    var importance = dragData.importance;
                    if (dropData) {
                        importance = dropData.importance;
                        if (dropPosition == 'before') {
                            importance += 10;
                        } else {
                            importance -= 10;
                        }
                    }

                    var params = {
                        id: dragData.id,
                        importance: importance,
                        sprintId: 0
                    };

                    Ext.Ajax.request({
                        url: '/UserStories/UpdateExt',
                        method: 'POST',
                        params: params,
                        callback: function () {
                            backlogGridPanel.store.load();
                        }
                    });
                }
            }
        },
        store: {
            xtype: 'jsonstore',
            autoLoad: true,
            proxy: new Ext.data.HttpProxy({
                url: '/Projects/GetBacklog?projectId=' + projectId,
                method: 'GET'
            }),
            fields:
            [
                'name',
                'importance',
                'estimate',
                'author_name',
                'author_surname',
                'categoryId',
                'categoryName'
            ],
            sorters:
            {
                property: 'importance',
                direction: 'DESC'
            },
            listeners: {
                update: function (thisStore, record, operation, eOpts) {
                    var params = { id: record.data['id'] };
                    for (var i = 0; i < eOpts.length; i++) {
                        params[eOpts[i]] = record.data[eOpts[i]];
                    }

                    Ext.Ajax.request({
                        url: '/UserStories/UpdateExt',
                        method: 'POST',
                        params: params,
                        callback: function () {
                            thisStore.load();
                        }
                    });

                }
            }
        },
        columns: [{
            xtype: 'rownumberer'
        }, {
            header: 'Название',
            dataIndex: 'name',
            width: 220,
            summaryType: 'count',
            summaryRenderer: function (value, summaryData, dataIndex) {
                return Ext.String.format('<b>Количество требований: {0}</b>', value);
            },
            filter: {
                type: 'string'
            },
            editor: {
                xtype: 'textfield',
                allowBlank: false
            }
        }, {
            header: 'Важность',
            dataIndex: 'importance',
            width: 60,
            filter: {
                type: 'numeric'
            },
            editor: {
                xtype: 'numberfield',
                hideTrigger: true,
                allowBlank: false
            }
        }, {
            header: 'Оценка',
            dataIndex: 'estimate',
            width: 50,
            xtype: 'templatecolumn',
            tpl: '{estimate} ч',
            filter: {
                type: 'numeric'
            },
            summaryType: 'sum',
            summaryRenderer: function (value, summaryData, dataIndex) {
                return Ext.String.format('<b>{0} ч</b>', value);
            },
            editor: {
                xtype: 'numberfield',
                hideTrigger: true,
                allowBlank: false
            }
        }, {
            header: 'Автор',
            dataIndex: 'author_name',
            xtype: 'templatecolumn',
            tpl: '{author_name} {author_surname}',
            filter: {
                type: 'string'
            }
        }, {
            header: 'Категория',
            dataIndex: 'categoryId',
            xtype: 'templatecolumn',
            tpl: '{categoryName}',
            width: 130,
            filter: {
                type: 'string'
            },
            editor: {
                xtype: 'combo',
                store: {
                    xtype: 'jsonstore',
                    autoLoad: true,
                    proxy: new Ext.data.HttpProxy({
                        url: '/UserStories/GetCategories?projectId=' + projectId + '&includeEmpty=' + true,
                        method: 'GET'
                    }),
                    fields:
                    [
                        'id',   //числовое значение - номер элемента
                        'name' //текст
                    ]
                },
                valueField: 'id',
                displayField: 'name',
                queryMode: 'remote',
            }
        }, {
            xtype: 'actioncolumn',
            width: 30,
            items: [{
                icon: '\\content\\images\\delete-icon.png',
                handler: function (grid, rowIndex, colIndex) {
                    // проверка на возможность удалит
                    // Ext.MessageBox.alert("Вы не можете удалить!");
                    Ext.MessageBox.show({
                        title: "Подтвердите действие",
                        msg: "Вы уверены что хотите удалить требование?",
                        buttons: Ext.Msg.YESNO,
                        closable: false,
                        fn: function (answer) {
                            if (answer == "yes") {
                                var us_id = grid.store.data.items[rowIndex].data.id;

                                var getDefault = new XMLHttpRequest();
                                getDefault.open('GET', '/UserStories/Delete/' + us_id, false);
                                getDefault.send();

                                backlogGridPanel.store.load();
                            }
                        }
                    });
                }
            }]
        }]
    });

    return backlogGridPanel;
}