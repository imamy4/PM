function GetUserStoriesGrid(sprintId, changeCallback) {
    var userStoriesGridPanel = Ext.create('Ext.grid.Panel', {
        stripeRows: true,
        border: false,
        plugins: [{
            ptype: 'rowediting',
            clicksToEdit: 2
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
                'name',
                'importance',
                'estimate',
                'executorId',
                'executorName',
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
                            if (changeCallback) {
                                changeCallback();
                            }
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
                allowBlank: false
            }
        }, {
            header: 'Оценка',
            dataIndex: 'estimate',
            width: 50,
            xtype: 'templatecolumn',
            tpl: '{estimate} ч',
            editor: {
                xtype: 'numberfield',
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
                        'id',   //числовое значение - номер элемента
                        'name' //текст
                    ]
                },
                valueField: 'id',
                displayField: 'name',
                queryMode: 'remote',
            }
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
                        'Id',   //числовое значение - номер элемента
                        'Name' //текст
                    ]
                },
                valueField: 'Id',
                displayField: 'Name',
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