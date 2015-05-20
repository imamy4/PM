Ext.onReady(function () {
    var mainWidth = Ext.get('desktop').getWidth();

    var currentSprintsGrid = GetSprintsGrid('/Projects/GetCurrentSprints?projectId=' + projectId),
        futureSprintsGrid = GetSprintsGrid('/Projects/GetFutureSprints?projectId=' + projectId),
        pastSprintsGrid = GetSprintsGrid('/Projects/GetPastSprints?projectId=' + projectId);

    var srintsAccordionPanel = Ext.create('Ext.Panel', {
        title: 'Спринты текущего проекта',
        bodyStyle: 'background:transparent;',
        region: 'west',
        width: mainWidth * 0.48,
        margin: '5 5 5 5',
        layout: 'accordion',
        dockedItems: [{
            xtype: 'toolbar',
            items: [
            {
                xtype: 'button',
                text: '<b>Добавить спринт</b>',
                handler: function () {
                    var createWin = GetCreateSprintWIndow();
                    createWin.projectId = projectId;
                    createWin.successCallback = function () {
                        currentSprintsGrid.store.load();
                        futureSprintsGrid.store.load();
                        pastSprintsGrid.store.load();
                    };
                    createWin.show();
                }
            }]
        }],
        items: [
            {
                xtype: 'panel',
                title: 'Текущие спринты',
                layout: 'fit',
                items: [currentSprintsGrid]
            },
            {
                xtype: 'panel',
                title: 'Будующие спринты',
                layout: 'fit',
                items: [futureSprintsGrid]
            },
            {
                xtype: 'panel',
                title: 'Завершенные спринты',
                layout: 'fit',
                items: [pastSprintsGrid]
            }],
    });

    var backlogGridPanel = Ext.create('Ext.grid.Panel', {
        stripeRows: true,
        border:false,
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
                        dropData = dropRow.data;

                    var importance = dropData.importance;
                    if (dropPosition == 'before') {
                        importance += 10;
                    } else {
                        importance -= 10;
                    }

                    var params = {
                        id: dragData.id,
                        importance: importance
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
                            }
                        }
                    });
                }
            }]
        }]
    });

    var backlogPanel = Ext.create('Ext.panel.Panel', {
        title: 'Бэклог',
        bodyStyle: 'background:transparent;',
        region: 'east',
        width: mainWidth * 0.48,
        margin: '5 5 5 5',
        layout: 'fit',
        dockedItems: [{
            xtype: 'toolbar',
            items: [
            {
                xtype: 'button',
                text: '<b>Добавить требование</b>',
                handler: function () {
                    var createWin = GetCreateUSWIndow();
                    createWin.projectId = projectId;
                    createWin.successCallback = function () {
                        backlogGridPanel.store.load();
                    };
                    createWin.show();
                }
            }]
        }],
        items: [backlogGridPanel]
    });

    Ext.create('Ext.panel.Panel', {
        renderTo: 'desktop',
        width: mainWidth,
        height: 450,
        border: false,
        bodyStyle: 'background:transparent;',
        layout: 'border',
        items: [srintsAccordionPanel, backlogPanel]
    });
});