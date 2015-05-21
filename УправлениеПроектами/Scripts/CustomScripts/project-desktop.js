Ext.onReady(function () {
    var mainWidth = Ext.get('desktop').getWidth();

    var currentSprintsGrid = GetSprintsGrid('/Projects/GetCurrentSprints?projectId=' + projectId),
        futureSprintsGrid = GetSprintsGrid('/Projects/GetFutureSprints?projectId=' + projectId),
        pastSprintsGrid = GetSprintsGrid('/Projects/GetPastSprints?projectId=' + projectId),
        backlogGridPanel = GetBacklogGrid(projectId);

    currentSprintsGrid.backlogGridPanel = backlogGridPanel;
    futureSprintsGrid.backlogGridPanel = backlogGridPanel;


    var srintsAccordionPanel = Ext.create('Ext.Panel', {
        title: 'Спринты текущего проекта',
        bodyStyle: 'background:transparent;border-radius: 0 0 10px 10px;',
        anchor: '100% 50%',
        frame: true,
        margin: '5 5 5 5',
        layout: 'accordion',
        dockedItems: [{
            xtype: 'toolbar',
            items: [
            {
                xtype: 'button',
                text: '<b>Добавить спринт</b>',
                handler: function () {
                    var createWin = GetCreateSprintWindow();
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

    var backlogPanel = Ext.create('Ext.panel.Panel', {
        title: 'Бэклог',
        bodyStyle: 'background:transparent;border-radius: 0 0 10px 10px;',
        anchor: '100% 50%',
        frame: true,
        margin: '5 5 5 5',
        layout: 'fit',
        dockedItems: [{
            xtype: 'toolbar',
            items: [
            {
                xtype: 'button',
                text: '<b>Добавить требование</b>',
                handler: function () {
                    var createWin = GetCreateUSWindow();
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

    Ext.create('Ext.Viewport', {
        renderTo: 'desktop',
        width: mainWidth,
        border: false,
        bodyStyle: 'background:transparent;',
        layout: 'anchor',
        items: [srintsAccordionPanel, backlogPanel]
    });
});