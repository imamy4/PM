function GetSprintsGrid(storeUrl) {
    var sprintsGridPanel = Ext.create('Ext.grid.Panel', {
        border: false,
        bodyStyle: 'background:transparent;',
        stripeRows: true,
        viewConfig: {
            plugins: {
                ptype: 'gridviewdragdrop',
                dropGroup: 'thisGridDDGroup'
            },
            listeners: {// node, какую строку тащу, на место какой ставлю, до или после встает
                drop: function (node, dragRowData, dropRow, dropPosition) {
                    var dragData = dragRowData.records[0].data,
                        dropData = dropRow.data;

                    var sprintId = dropData.id;

                    var params = {
                        id: dragData.id,
                        sprintId: sprintId
                    };

                    Ext.Ajax.request({
                        url: '/UserStories/UpdateExt',
                        method: 'POST',
                        params: params,
                        callback: function (options, success, response) {
                            sprintsGridPanel.store.load();
                            if (sprintsGridPanel.backlogGridPanel) {
                                sprintsGridPanel.backlogGridPanel.store.load();
                            }
                            var result = Ext.JSON.decode(response.responseText);
                            if (!result.success) {
                                Ext.Msg.alert('Предупреждение', 'Произошла ошибка');
                            }
                        },
                        failure: function () {
                            sprintsGridPanel.store.load();
                            if (sprintsGridPanel.backlogGridPanel) {
                                sprintsGridPanel.backlogGridPanel.store.load();
                            }
                            Ext.Msg.alert('Произошла ошибка');
                        }
                    });
                }
            }
        },
        plugins: [{
            ptype: 'rowediting',
            clicksToEdit: 2
        }],
        store: {
            xtype: 'jsonstore',
            autoLoad: true,
            proxy: new Ext.data.HttpProxy({
                url: storeUrl,
                method: 'GET'
            }),
            fields:
            [
                'name',
                'dateStart',
                'dateFinish',
                'usCount',
                'newUsCount',
                'processedUsCount',
                'resolvedUsCount',
                'assigmentUsCount',
                'notAssigmentusCount',
                'sprintPower',
                'sumEstimate'
            ],
            sorters:
            {
                property: 'dateFinish',
                direction: 'ASC'
            },
            listeners: {
                update: function (thisStore, record, operation, eOpts) {
                    var params = { id: record.data['id'] };
                    for (var i = 0; i < eOpts.length; i++) {
                        params[eOpts[i]] = record.data[eOpts[i]];
                    }

                    Ext.Ajax.request({
                        url: '/Sprints/UpdateExt',
                        method: 'POST',
                        params: params,
                        callback: function () {
                            thisStore.load();
                        },
                        failure: function () {
                            thisStore.load();
                            Ext.Msg.alert('Произошла ошибка');
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
            width: 120,
            editor: {
                xtype: 'textfield',
                allowBlank: false
            }
        }, {
            header: 'Дата начала',
            dataIndex: 'dateStart',
            xtype: 'datecolumn',
            format: 'd-m-Y',
            flex: 1,
            editor: {
                xtype: 'datefield',
                allowBlank: false
            }
        }, {
            header: 'Дата конца',
            dataIndex: 'dateFinish',
            xtype: 'datecolumn',
            format: 'd-m-Y',
            flex: 1,
            editor: {
                xtype: 'datefield',
                allowBlank: false
            }
        }, {
            header: 'Всего задач',
            dataIndex: 'usCount',
            flex: 1,
        }, {
            header: 'Новыз задач',
            dataIndex: 'newUsCount',
            flex: 1,
        }, {
            header: 'Задач в работе',
            dataIndex: 'resolvedUsCount',
            flex: 1,
        }, {
            header: 'Решенных задач',
            dataIndex: 'resolvedUsCount',
            flex: 1,
        }, {
            header: 'Назначенных задач',
            dataIndex: 'assigmentUsCount',
            flex: 1,
        }, {
            header: 'Неназначенных задач',
            dataIndex: 'notAssigmentusCount',
            flex: 1,
        }, {
            header: 'Мощность спринта',
            dataIndex: 'sprintPower',
            flex: 1,
        }, {
            header: 'Суммарная оценка задач',
            dataIndex: 'sumEstimate',
            flex: 1,
        }, {
            xtype: 'actioncolumn',
            width: 30,
            items: [{
                icon: '\\content\\images\\list-icon.png',
                handler: function (grid, rowIndex, colIndex) {
                    var sprintId = grid.store.data.items[rowIndex].data.id;
                    var name = grid.store.data.items[rowIndex].data.name;

                    GetSprintWindow(sprintId, 'Спринт: ' + name, function () { sprintsGridPanel.store.load(); }).show();
                }
            }]
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
                        msg: "Вы уверены что хотите удалить спринт?",
                        buttons: Ext.Msg.YESNO,
                        closable: false,
                        fn: function (answer) {
                            if (answer == "yes") {
                                var us_id = grid.store.data.items[rowIndex].data.id;

                                var getDefault = new XMLHttpRequest();
                                getDefault.open('GET', '/Sprints/Delete/' + us_id, false);
                                getDefault.send();

                                grid.store.load();
                            }
                        }
                    });
                }
            }]
        }]
    });

    return sprintsGridPanel;
}