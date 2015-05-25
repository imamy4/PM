function getUserDesktopTasksGridPanel(storeUrl) {

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

    var executorCombobox = Ext.create('Ext.form.field.ComboBox', {
        store: {
            xtype: 'jsonstore',
            autoLoad: true,
            proxy: new Ext.data.HttpProxy({
                url: '/Projects/GetUsers?projectId=null&includeEmpty=true',
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

    var categoryCombobox = Ext.create('Ext.form.field.ComboBox', {
        store: {
            xtype: 'jsonstore',
            autoLoad: true,
            proxy: new Ext.data.HttpProxy({
                url: '/UserStories/GetCategories?projectId=null&includeEmpty=true',
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

    var userDesktopTasksGridPanel = Ext.create('Ext.grid.Panel', {
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

                    executorCombobox.store.proxy.url = '/Projects/GetUsers?projectId=' + e.record.data.projectId + '&includeEmpty=true';
                    executorCombobox.store.load();

                    categoryCombobox.store.proxy.url = '/UserStories/GetCategories?projectId=' + e.record.data.projectId + '&includeEmpty=true';
                    categoryCombobox.store.load();
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
                url: storeUrl,
                method: 'GET'
            }),
            fields:
            [
                'id',
                'name',
                'projectId',
                'projectName',
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
                sorterFn: function (o1, o2) {
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
            header: 'Проект',
            dataIndex: 'projectName',
            summaryType: 'count',
            summaryRenderer: function (value, summaryData, dataIndex) {
                return Ext.String.format('<b>Количество требований: {0}</b>', value);
            },
            width: 140
        }, {
            header: 'Название',
            dataIndex: 'name',
            width: 140,
            renderer: function (value, metaData, record) {
                if (record.data.statusIsResolved) {
                    return Ext.String.format('<strike>{0}</strike>', value);
                }
                return value;
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
            width: 100,
            xtype: 'templatecolumn',
            tpl: '{spentTime} ч',
            summaryType: 'sum',
            summaryRenderer: function (value, summaryData, dataIndex) {
                return Ext.String.format('<b>{0} ч</b>', value);
            }
        }, {
            header: 'Исполнитель',
            dataIndex: 'executorId',
            xtype: 'templatecolumn',
            tpl: '{executorName}',
            width: 130,
            editor: executorCombobox
        }, {
            header: 'Автор',
            dataIndex: 'author_name',
            xtype: 'templatecolumn',
            tpl: '{author_name} {author_surname}'
        }, {
            header: 'Статус',
            dataIndex: 'statusId',
            xtype: 'templatecolumn',
            tpl: '{statusName}',
            width: 130,
            editor: statusCombobox
        }, {
            header: 'Категория',
            dataIndex: 'categoryId',
            xtype: 'templatecolumn',
            tpl: '{categoryName}',
            width: 130,
            editor: categoryCombobox
        }]
    });

    return userDesktopTasksGridPanel;
}


function getUserDesktopSpentTimeGridPanel() {
   
    var userDesktopSpentTimeGridPanel = Ext.create('Ext.grid.Panel', {
        stripeRows: true,
        border: false,
        features: [{
            ftype: 'summary'
        }],
        store: {
            xtype: 'jsonstore',
            autoLoad: true,
            proxy: new Ext.data.HttpProxy({
                url: '/User/GetLastMonthActivity',
                method: 'GET'
            }),
            fields:
            [
                'projectName',
                'userStoryName',
                'dateStart',
                'dateFinish',
                'activityTime'
            ],
            sorters:
            {
                property: 'dateFinish',
                direction: 'DESC'
            }
        },
        columns: [{
            xtype: 'rownumberer'
        }, {
            header: 'Проект',
            dataIndex: 'projectName',
            width: 100
        }, {
            header: 'Требования',
            dataIndex: 'userStoryName',
            width: 100
        }, {
            header: 'Время начала',
            dataIndex: 'dateStart',
            xtype: 'datecolumn',
            format: 'd-m-Y h:i',
            flex: 1,
        }, {
            header: 'Время конца',
            dataIndex: 'dateFinish',
            xtype: 'datecolumn',
            format: 'd-m-Y h:i',
            flex: 1,
        }, {
            header: 'Затраченное время',
            dataIndex: 'activityTime',
            xtype: 'templatecolumn',
            tpl: '{activityTime} ч',
            summaryType: 'sum',
            summaryRenderer: function (value, summaryData, dataIndex) {
                return Ext.String.format('<b>Всего затрачено: {0} ч</b>', value);
            },
            width: 140
        }


        ]
    });

    return userDesktopSpentTimeGridPanel;

}


function getUserDesktopProjectInfoGridPanel() {

    var userDesktopProjectInfoGridPanel = Ext.create('Ext.grid.Panel', {
        stripeRows: true,
        border: false,
        store: {
            xtype: 'jsonstore',
            autoLoad: true,
            proxy: new Ext.data.HttpProxy({
                url: '/Projects/GetProjectsWithStats',
                method: 'GET'
            }),
            fields:
            [
                'id',
                'name',
                'dateStart',
                'dateFinish',
                'usCount',
                'newUsCount',
                'processedUsCount',
                'resolvedUsCount',
                'assigmentUsCount',
                'notAssigmentusCount',
                'sumEstimate'
            ],
            sorters:
            {
                property: 'dateFinish',
                direction: 'ASC'
            }
        },
        columns: [{
            xtype: 'rownumberer'
        }, {
            header: 'Проект',
            dataIndex: 'name',
            width: 100
        }, {
            header: 'Дата начала',
            dataIndex: 'dateStart',
            xtype: 'datecolumn',
            format: 'd-m-Y',
            flex: 1,
        }, {
            header: 'Дата конца',
            dataIndex: 'dateFinish',
            xtype: 'datecolumn',
            format: 'd-m-Y',
            flex: 1,
        }, {
            header: 'Всего задач',
            dataIndex: 'usCount',
            flex: 1,
        }, {
            header: 'Новых задач',
            dataIndex: 'newUsCount',
            flex: 1,
        }, {
            header: 'Задач в работе',
            dataIndex: 'processedUsCount',
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
            header: 'Суммарная оценка задач',
            dataIndex: 'sumEstimate',
            flex: 1,
        }, {
            xtype: 'actioncolumn',
            width: 30,
            items: [{
                icon: '\\content\\images\\link-icon.png',
                handler: function (grid, rowIndex, colIndex) {
                    var projectId = grid.store.data.items[rowIndex].data.id;
                    document.location.href = '/Projects/Desktop/' + projectId;
                }
            }]
        }]
    });

    return userDesktopProjectInfoGridPanel;
}