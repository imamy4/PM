Ext.onReady(function () {
    var assigmentTasksGrid = getUserDesktopTasksGridPanel('/User/GetAssignmentedUserStories');
    var createdTasksGrid = getUserDesktopTasksGridPanel('/User/GetCreatedUserStories');
    var spentTimeGrid = getUserDesktopSpentTimeGridPanel();
    var statsPanel = getUserDesktopProjectInfoGridPanel();
    var userTasksPanel = Ext.create('Ext.Panel', {
        title: 'Требования',
        bodyStyle: 'border-radius: 0 0 10px 10px;',
        flex: 1,
        height: '100%',
        frame: true,
        margin: '5 5 5 5',
        layout: 'accordion',
        items: [
            {
                xtype: 'panel',
                title: 'Требования на пользователе',
                layout: 'fit',
                items: [assigmentTasksGrid]
            },
            {
                xtype: 'panel',
                title: 'Требования созданные пользователем',
                layout: 'fit',
                items: [createdTasksGrid]
            }],
    });
    var months = ['январе', 'феврале', 'марте', 'апреле', 'мае', 'июне', 'июле', 'августе', 'сентябре', 'октябре', 'ноябре', 'декабре'];
    var day = new Date();
    var currentMonth = months[day.getMonth()];
    var spentTimePanel = Ext.create('Ext.panel.Panel', {
        title: 'Время затраченное в ' + currentMonth,
        bodyStyle: 'border-radius: 0 0 10px 10px;',
        flex: 1,
        frame: true,
        height: '100%',
        margin: '5 5 5 5',
        layout: 'fit',
        items: [spentTimeGrid]
    });

    var topPanel = Ext.create('Ext.container.Container', {
        anchor: '100% 60%',
        layout: {
            type: 'hbox'
        },
        items: [userTasksPanel, spentTimePanel]
    });


    var projectInfoPanel = Ext.create('Ext.panel.Panel', {
        title: 'Информация по проектам',
        bodyStyle: 'background:transparent;border-radius: 0 0 10px 10px;',
        anchor: '100% 40%',
        frame: true,
        margin: '5 5 5 5',
        layout: 'fit',
        items: [statsPanel]
});



    Ext.create('Ext.Viewport', {
        renderTo: 'desktop',
        border: false,
        layout: 'anchor',
        items: [topPanel, projectInfoPanel]
    });
});