function GetMainPanel(config) {

    var mainMenuPanel = Ext.create('Ext.panel.Panel', {
        frame: false,
        layout: 'anchor',
        items: config.items
    });

    // получаем инфу о текущей 
    Ext.Ajax.request({
        url: '/User/GetCurrentActivity',
        success: function (response) {
            var text = response.responseText;
            result = Ext.JSON.decode(text);

            mainMenuPanel.addDocked(CreateMenuToolbar(mainMenuPanel, {
                userStory: result.userStory,
                isHomePage: config.isHomePage,
                completeActivityCallback: config.completeActivityCallback
            }));
        }
    });


    return mainMenuPanel;
}

function CreateMenuToolbar(mainMenuPanel, config) {
    var userStory = config ? config.userStory : null;
    var isHomePage = config ? config.isHomePage : false;
    var completeActivityCallback = config ? config.completeActivityCallback : null;

    var toolbarItems = [];

    if (userStory) {
        var dateStart = new Date(userStory.dateStart);
        toolbarItems.push({
            xtype: 'button',
            text: 'Текущая задача: ' + userStory.name,
            handler: function () {
                document.location.href = "/UserStories/Desktop/" + userStory.id;
            }
        });
        toolbarItems.push('|');
        toolbarItems.push({
            xtype: 'button',
            text: 'Принята в работу: ' + Ext.Date.format(dateStart, 'd-m-Y H:i')
        });
        toolbarItems.push('|');
        toolbarItems.push({
            xtype: 'button',
            text: 'Времени в работе: ' + GetTimeStampString(dateStart),
        });
        toolbarItems.push('|');
        toolbarItems.push({
            xtype: 'button',
            text: 'Закончить учет времени',
            handler: function () {
                Ext.Ajax.request({
                    url: '/User/CompleteCurrentActivity',
                    callback: function (response) {
                        mainMenuPanel.removeDocked(mainMenuPanel.dockedItems.items[0]);
                        mainMenuPanel.addDocked(CreateMenuToolbar(mainMenuPanel, null, isHomePage));
                        if (completeActivityCallback) {
                            completeActivityCallback();
                        }
                    }
                });
            }
        });
    }

    toolbarItems.push('->');
    if (!isHomePage) {
        toolbarItems.push({
            xtype: 'button',
            text: 'Перейти на рабочую страницу',
            handler: function () {
                document.location.href = '/';
            }
        });
        toolbarItems.push('|');
    }

    toolbarItems.push({
        xtype: 'button',
        text: '<b>Выйти из системы</b>',
        handler: function () {
            Ext.MessageBox.wait('Выход из системы...', 'Пожалуйста подождите');
            Ext.Ajax.request({
                url: '/Login/Logout',
                callback: function (response) {
                    document.location.href = '/';
                }
            });
        }
    });

    var menuToolbar = Ext.create('Ext.toolbar.Toolbar', {
        items: toolbarItems
    });

    return menuToolbar;
}

function GetTimeStampString(dateStart) {
    var milliseconds = new Date() - dateStart;
    var hours = Math.floor(milliseconds / 1000 / 3600);
    minutes = Math.round(milliseconds / 1000 / 60 - hours * 60);
    if (minutes < 10) {
        minutes = '0' + minutes.toString();
    }
    return hours + ':' + minutes;
}