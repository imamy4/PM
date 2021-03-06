function GetMainPanel(config) {

    var mainMenuPanel = Ext.create('Ext.panel.Panel', {
        frame: false,
        layout: config.layout ? config.layout : 'anchor',
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
                user: result.user,
                isHomePage: config.isHomePage,
                isUSPage: config.isUSPage,
                completeActivityCallback: config.completeActivityCallback
            }));
        }
    });


    return mainMenuPanel;
}

function CreateMenuToolbar(mainMenuPanel, config) {
    var userStory = config ? config.userStory : null;
    var user = config ? config.user : null;
    var isHomePage = config ? config.isHomePage : false;
    var isUSPage = config ? config.isUSPage : false;
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
        var timingButton = Ext.create('Ext.Button', {
            text: 'Времени в работе: ' + GetTimeStampString(dateStart),
        });
        var intervalId = setInterval(function () {
            timingButton.setText('Времени в работе: ' + GetTimeStampString(dateStart));
        }, 1000);

        toolbarItems.push(timingButton);
        toolbarItems.push('|');
        toolbarItems.push({
            xtype: 'button',
            text: 'Закончить учет времени',
            handler: function () {
                clearInterval(intervalId);
                Ext.Ajax.request({
                    url: '/User/CompleteCurrentActivity',
                    callback: function (response) {
                        Ext.Ajax.request({
                            url: '/User/GetCurrentActivity',
                            success: function (response) {
                                var text = response.responseText;
                                result = Ext.JSON.decode(text);
                                mainMenuPanel.removeDocked(mainMenuPanel.dockedItems.items[0]);
                                mainMenuPanel.addDocked(CreateMenuToolbar(mainMenuPanel, {
                                    userStory: result.userStory,
                                    user: result.user,
                                    isHomePage: config.isHomePage,
                                    completeActivityCallback: config.completeActivityCallback
                                }));
                            }
                        });
                    }
                });
            }
        });
    } else {
        if (isUSPage) {
            toolbarItems.push({
                xtype: 'button',
                text: 'Взять требование в работу',
                handler: function () {
                    Ext.Ajax.request({
                        url: '/User/StrartActivity?userStoryId=' + userStoryId,
                        callback: function (response) {
                            Ext.Ajax.request({
                                url: '/User/GetCurrentActivity',
                                success: function (response) {
                                    var text = response.responseText;
                                    result = Ext.JSON.decode(text);
                                    mainMenuPanel.removeDocked(mainMenuPanel.dockedItems.items[0]);
                                    mainMenuPanel.addDocked(CreateMenuToolbar(mainMenuPanel, {
                                        userStory: result.userStory,
                                        user: result.user,
                                        isHomePage: config.isHomePage,
                                        completeActivityCallback: config.completeActivityCallback
                                    }));
                                }
                            });
                        }
                    });
                }
            });
        }
    }

    toolbarItems.push('->');
    
        toolbarItems.push({
            xtype: 'button',
            text: user.userName,
            handler: function () {
                if (!isHomePage) {
                    document.location.href = '/';
                }
            }
        });
        toolbarItems.push('|');

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
    var minutes = Math.floor(milliseconds / 1000 / 60 - hours * 60);
    var seconds = Math.floor(milliseconds / 1000 - hours * 3600 - minutes * 60);
    if (minutes < 10) {
        minutes = '0' + minutes.toString();
    }
    if (seconds < 10) {
        seconds = '0' + seconds.toString();
    }
    return hours + ':' + minutes + ':' + seconds;
}