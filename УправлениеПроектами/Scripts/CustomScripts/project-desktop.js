Ext.onReady(function () {

    var currentSprintsGrid = GetSprintsGrid({ storeUrl: '/Projects/GetCurrentSprints?projectId=' + projectId, projectId: projectId }),
        futureSprintsGrid = GetSprintsGrid({ storeUrl: '/Projects/GetFutureSprints?projectId=' + projectId, projectId: projectId }),
        pastSprintsGrid = GetSprintsGrid({ storeUrl: '/Projects/GetPastSprints?projectId=' + projectId, projectId: projectId }),
        backlogGridPanel = GetBacklogGrid(projectId);

    currentSprintsGrid.backlogGridPanel = backlogGridPanel;
    futureSprintsGrid.backlogGridPanel = backlogGridPanel;


    var srintsAccordionPanel = Ext.create('Ext.Panel', {
        title: 'Спринты текущего проекта',
        bodyStyle: 'border-radius: 0 0 10px 10px;',
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
                    var successCallback = function () {
                        backlogGridPanel.store.load();
                    };

                    GetCreateUSWindow(successCallback, { projectId: projectId }).show();
                }
            }]
        }],
        items: [backlogGridPanel]
    });

    var mainMenuPanel = GetMainPanel({
        items: [srintsAccordionPanel, backlogPanel],
        isHomePage: false
    });

    Ext.create('Ext.Viewport', {
        renderTo: 'desktop',
        border: false,
        layout: 'fit',
        items: [mainMenuPanel]
    });
});