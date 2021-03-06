function GetUserStoriesGrid(sprintId, changeCallback) {

    var statusCombobox = Ext.create('Ext.form.field.ComboBox', {
        store: {
            xtype: 'jsonstore',
            autoLoad: true,
            proxy: new Ext.data.HttpProxy({
                url: '/UserStories/GetStatusJumps?statusId=null', 
                method: 'GET'
            }),
            fields:
            [
                'id',
                'name'
            ]
        },
        valueField: 'id',
        displayField: 'name',
        queryMode: 'remote'
    });

    var userStoriesGridPanel = Ext.create('Ext.grid.Panel', {
        stripeRows: true,
        border: false,
        features: [{
            ftype: 'summary'
        }],
        plugins: [{
            ptype: 'rowediting',
            clicksToEdit: 2,
            listeners: {
                beforeedit: function (editor, e, eOpts) {
                    statusCombobox.store.proxy.url = '/UserStories/GetStatusJumps?statusId=' + e.record.data.statusId;
                    statusCombobox.store.load();
                }
            }
        }],
        viewConfig: {
            plugins: {
                ptype: 'gridviewdragdrop',
                dragGroup: 'thisGridDDGroup',
            }
        },
        store: {
            xtype: 'jsonstore',
            autoLoad: true,
            proxy: new Ext.data.HttpProxy({
                url: '/Sprints/GetUserStories/' + sprintId,
                method: 'GET'
            }),
            fields:
            [
                'id',
                'name',
                'importance',
                'estimate',
                'spentTime',
                'executorId',
                'executorName',
                'statusId',
                'statusName',
                'statusIsResolved',
                'author_name',
                'author_surname',
                'categoryId',
                'categoryName'
            ],
            sorters: [{
                sorterFn: function(o1, o2){
                    var isResolved1 = o1.get('statusIsResolved'),
                        isResolved2 = o2.get('statusIsResolved'),
                        importance1 = o1.get('importance'),
                        importance2 = o2.get('importance');

                    if (isResolved1) {
                        return 1;
                    }
                    if (isResolved2) {
                        return -1;
                    }

                    return importance1 > importance2 ? -1 : 1;
                }
            }],
            listeners: {
                update: function (thisStore, record, operation, eOpts) {
                    statusCombobox.store.load();

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
                            if (changeCallback) {
                                changeCallback();
                            }
                        },
                        failure: function () {
                            thisStore.load();
                            if (changeCallback) {
                                changeCallback();
                            }
                            Ext.Msg.alert('Предупреждение', 'Произошла ошибка');
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
            renderer: function (value, metaData, record) {
                var result = value;
                if (record.data.statusIsResolved) {
                    result = Ext.String.format('<strike>{0}</strike>', value);
                }
                return Ext.String.format('<a href={1}>{0}</a>', result, '/UserStories/Desktop/' + record.data.id);
            },
            summaryType: 'count',
            summaryRenderer: function (value, summaryData, dataIndex) {
                return Ext.String.format('<b>Количество требований: {0}</b>', value);
            },
            editor: {
                xtype: 'textfield',
                allowBlank: false
            }
        }, {
            header: 'Важность',
            dataIndex: 'importance',
            width: 60,
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
            header: 'Затрачено',
            dataIndex: 'spentTime',
            width: 50,
            xtype: 'templatecolumn',
            tpl: '{spentTime} ч',
            renderer: function (value, metaData, record) {
                if (record.data.spentTime > record.data.estimate) {
                    return Ext.String.format('<b><span style="color:rgb(201, 49, 60);">{0}</span></b>', value);
                }
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
            header: 'Исполнитель',
            dataIndex: 'executorId',
            xtype: 'templatecolumn',
            tpl: '{executorName}',
            width: 130,
            editor: {
                xtype: 'combo',
                store: {
                    xtype: 'jsonstore',
                    autoLoad: true,
                    proxy: new Ext.data.HttpProxy({
                        url: '/Projects/GetUsers?projectId=' + projectId + '&includeEmpty=' + true,
                        method: 'GET'
                    }),
                    fields:
                    [
                        'id',
                        'name'
                    ]
                },
                valueField: 'id',
                displayField: 'name',
                queryMode: 'remote',
            }
        }, {
            header: 'Статус',
            dataIndex: 'statusId',
            xtype: 'templatecolumn',
            tpl: '{statusName}',
            width: 130,
            editor: statusCombobox
        }, {
            header: 'Автор',
            dataIndex: 'author_name',
            xtype: 'templatecolumn',
            tpl: '{author_name} {author_surname}'
        }, {
            header: 'Категория',
            dataIndex: 'categoryId',
            xtype: 'templatecolumn',
            tpl: '{categoryName}',
            width: 130,
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
                        'id',
                        'name'
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
                                if (changeCallback) {
                                    changeCallback();
                                }
                            }
                        }
                    });
                }
            }]
        }]
    });

    return userStoriesGridPanel;
}