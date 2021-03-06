Ext.require('Ext.chart.*');
Ext.require('Ext.layout.container.Fit');

Ext.onReady(function () {

    var burndownChart = GetBurndownChart('/Charts/GetBurndownData/?sprintId=' + sprintId);

    var userStoriesPanel = Ext.create('Ext.panel.Panel', {
        title: 'Задачи спринта',
        anchor: '60% 100%',
        bodyStyle: 'border-radius: 0 0 10px 10px;',
        layout: 'fit',
        frame: true,
        flex: 1,
        margin: '5 5 5 5',
        buttons: [{
            xtype: 'button',
            text: 'Перейти на страницу проекта',
            handler: function () {
                location.href = '/Projects/Desktop/' + projectId;
            }
        }],
        items: [GetUserStoriesGrid(sprintId)]
    });

    var westPanel = Ext.create('Ext.panel.Panel', {
        layout: 'fit',
        region: 'west',
        width: '60%',
        items: [userStoriesPanel],
    });

    var eastPanel = Ext.create('Ext.panel.Panel', {
        layout: 'fit',
        region: 'east',
        width: '40%',
        items: [burndownChart],
    });

    var mainMenuPanel = GetMainPanel({
        completeActivityCallback: function () { spentTimeGrid.store.load(); },
        isHomePage: false,
        isUSPage: true,
        layout: 'border',
        items: [westPanel, eastPanel]
    });

    Ext.create('Ext.Viewport', {
        renderTo: 'desktop',
        border: false,
        layout: 'fit',
        items: [mainMenuPanel]
    });
});