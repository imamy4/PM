Ext.onReady(function () {
    var userStoryPanel = Ext.create('Ext.Panel', {
        title: 'Требование',
        bodyStyle: 'border-radius: 0 0 10px 10px;',
        layout: 'fit',
        frame: true,
        flex: 1,
        margin: '5 5 5 5',
        items: [GetCreateUSForm(null, null, '', defaultValues)]
    });
    
    var activityPanel = Ext.create('Ext.panel.Panel', {
        title: 'Списания времени',
        anchor: '100% 50%',
        bodyStyle: 'border-radius: 0 0 10px 10px;',
        frame: true,
        flex: 1,
        margin: '5 5 5 5',
    });

    var assigmentPanel = Ext.create('Ext.panel.Panel', {
        title: 'Назначения',
        anchor: '100% 50%',
        bodyStyle: 'border-radius: 0 0 10px 10px;',
        frame: true,
        flex: 1,
        margin: '5 5 5 5',
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
        items: [activityPanel, assigmentPanel],
    });

    var mainMenuPanel = GetMainPanel({
        items: [westPanel, eastPanel],
        completeActivityCallback: function () { alert("Нужно перезегружать стор у грида списанного времени") },
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