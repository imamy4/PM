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

    var mainMenuPanel = Ext.create('Ext.panel.Panel', {
        frame: false,
        layout: 'anchor',
        dockedItems: [{
            xtype: 'toolbar',
            items: [
                {
                    xtype: 'button',
                    text: 'Текущая задача: Сделать пагинацию гридов',
                },
                '|',
                {
                    xtype: 'button',
                    text: 'Начата: 14-05-2015 12:38',
                },
                '|',
                {
                    xtype: 'button',
                    text: 'Затрачено: 1:48',
                },
                '|',
                {
                    xtype: 'button',
                    text: 'Закончить учет времени',
                },
                '->',
                {
                    xtype: 'button',
                    text: '<b>Выйти из системы</b>',
                    handler: function () {
                        Ext.MessageBox.wait('Выход из системы...', 'Пожалуйста подождите');
                        Ext.Ajax.request({
                            url: '/Login/Logout',
                            callback: function(response){
                                document.location.href = '/';
                            }
                        });
                    }
                }]
        }],
        items: [topPanel, projectInfoPanel]
    });

    Ext.create('Ext.Viewport', {
        renderTo: 'desktop',
        border: false,
        layout: 'fit',
        items: [mainMenuPanel]
    });
});