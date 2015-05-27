Ext.onReady(function () {

    var assigmentsGrid = GetAssigmentsGridPanel('/UserStories/GetAllAssigments?userStoryId=' + userStoryId);

    var userStoryPanel = Ext.create('Ext.Panel', {
        title: 'Требование',
        bodyStyle: 'border-radius: 0 0 10px 10px;',
        layout: 'fit',
        frame: true,
        flex: 1,
        margin: '5 5 5 5',
        items: [GetCreateUSForm(null, null, '', defaultValues)]
    });

    var statusPanel = Ext.create('Ext.panel.Panel', {
        title: 'Статус',
        anchor: '100% 20%',
        bodyStyle: 'border-radius: 0 0 10px 10px;',
        frame: true,
        flex: 1,
        layout: 'hbox',
        margin: '5 5 5 5',
        items: [{
            xtype: 'button',
            text: defaultValues.executorId != 0 ? 'Назначена на:' : 'Не назачена',
            margin: '15 0 15 15'
        }, {
            xtype: 'combo',
            width: 120,
            store: {
                xtype: 'jsonstore',
                autoLoad: true,
                proxy: new Ext.data.HttpProxy({
                    url: '/Projects/GetUsers?projectId=' + defaultValues.projectId + '&includeEmpty=' + true,
                    method: 'GET'
                }),
                fields:
                [
                    'id',
                    'name'
                ]
            },
            listeners: {
                change: function (thisCombo, newValue) {
                    Ext.Ajax.request({
                        url: '/UserStories/UpdateExt',
                        method: 'POST',
                        params: { id: userStoryId, executorId: newValue},
                        callback: function () {
                            assigmentsGrid.store.load();
                            if (changeCallback) {
                                changeCallback();
                            }
                        },
                        failure: function () {
                            if (changeCallback) {
                                changeCallback();
                            }
                            Ext.Msg.alert('Предупреждение', 'Произошла ошибка');
                        }
                    });
                }
            },
            value: defaultValues.executorId,
            valueField: 'id',
            displayField: 'name',
            queryMode: 'remote',
            margin: '15 15 15 5'
        }, {
            xtype: 'button',
            text: 'В статусе:',
            margin: '15 0 15 15'
        }, {
            xtype: 'combo',
            width: 100,
            store: {
                xtype: 'jsonstore',
                autoLoad: true,
                proxy: new Ext.data.HttpProxy({
                    url: '/UserStories/GetStatusJumps?statusId=' + defaultValues.statusId,
                    method: 'GET'
                }),
                fields:
                [
                    'id',
                    'name'
                ]
            },
            listeners: {
                change: function (thisCombo, newValue) {
                    Ext.Ajax.request({
                        url: '/UserStories/UpdateExt',
                        method: 'POST',
                        params: { id: userStoryId, statusId: newValue },
                        callback: function () {
                            if (changeCallback) {
                                changeCallback();
                            }
                        },
                        failure: function () {
                            if (changeCallback) {
                                changeCallback();
                            }
                            Ext.Msg.alert('Предупреждение', 'Произошла ошибка');
                        }
                    });
                }
            },
            value: defaultValues.statusId,
            valueField: 'id',
            displayField: 'name',
            queryMode: 'remote',
            margin: '15 15 15 15'
        }, {
            xtype: 'button',
            text: 'Автор: ' + defaultValues.authorName,
            margin: '15 15 15 5'
        }]
    });

    var assigmentPanel = Ext.create('Ext.panel.Panel', {
        title: 'Назначения',
        anchor: '100% 40%',
        bodyStyle: 'border-radius: 0 0 10px 10px;',
        frame: true,
        flex: 1,
        margin: '5 5 5 5',
        items: [assigmentsGrid]
    });

    var spentTimeGrid = GetUserDesktopSpentTimeGridPanel({ userStoryName: true, projectName: true }, '/UserStories/GetAllActivity?userStoryId=' + userStoryId);
    var activityPanel = Ext.create('Ext.panel.Panel', {
        title: 'Списания времени',
        anchor: '100% 40%',
        bodyStyle: 'border-radius: 0 0 10px 10px;',
        frame: true,
        flex: 1,
        margin: '5 5 5 5',
        items: [spentTimeGrid]
    });

    var westPanel = Ext.create('Ext.panel.Panel', {
        layout: 'fit',
        region: 'west',
        width: '50%',
        items: [userStoryPanel],
    });

    var eastPanel = Ext.create('Ext.panel.Panel', {
        layout: 'anchor',
        region: 'east',
        width: '50%',
        items: [statusPanel, assigmentPanel, activityPanel],
    });

    var mainMenuPanel = GetMainPanel({
        items: [westPanel, eastPanel],
        completeActivityCallback: function () { spentTimeGrid.store.load(); },
        isHomePage: false,
        isUSPage: true,
        layout: 'border'
    });

    Ext.create('Ext.Viewport', {
        renderTo: 'desktop',
        border: false,
        layout: 'fit',
        items: [mainMenuPanel]
    });
});

function GetAssigmentsGridPanel(storeUrl) {
    var columns = [{ xtype: 'rownumberer' }];

    columns.push({
        header: 'Пользователь',
        dataIndex: 'userName',
        width: 120
    });
    columns.push({
        header: 'Время назначения',
        dataIndex: 'dateStart',
        xtype: 'datecolumn',
        format: 'd-m-Y H:i',
        flex: 1,
        width: 80
    });
    columns.push({
        header: 'Время снятия',
        dataIndex: 'dateFinish',
        xtype: 'datecolumn',
        format: 'd-m-Y H:i',
        flex: 1,
        width: 80
    });

    var assigmentsGridPanel = Ext.create('Ext.grid.Panel', {
        stripeRows: true,
        border: false,
        features: [{
            ftype: 'summary'
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
                'userName',
                'dateStart',
                'dateFinish',
            ],
            sorters:
            {
                property: 'dateStart',
                direction: 'DESC'
            }
        },
        columns: columns
    });

    return assigmentsGridPanel;
}