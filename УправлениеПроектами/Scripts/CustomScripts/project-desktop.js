Ext.onReady(function () {
    var mainWidth = Ext.get('desktop').getWidth();

    var srintsAccordionPanel = Ext.create('Ext.Panel', {
        title: 'Спринты',
        bodyStyle: 'background:transparent;',
        region: 'west',
        width: mainWidth * 0.48,
        margin: '5 5 5 5',
        layout: 'accordion',
        items: [
            {
                xtype: 'panel',
                title: 'Текущие спринты',
                html: 'Произведения Л. Толстого: ....'
            },
            {
                xtype: 'panel',
                title: 'Будующие спринты',
                html: 'Произведения Ф. Достоевского: ...'
            },
            {
                xtype: 'panel',
                title: 'Завершенные спринты',
                html: 'Произведения И. Тургенева: ...'
            }],
    });


    var backlogGridPanel = Ext.create('Ext.grid.Panel', {
        title: 'Бэклог',
        bodyStyle: 'background:transparent;',
        region: 'east',
        width: mainWidth * 0.48,
        margin: '5 5 5 5',
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
                'author'
            ]
        },
        columns: [{
            header: 'Название',
            dataIndex: 'name'
        }, {
            header: 'Важность',
            dataIndex: 'importance'
        }, {
            header: 'Оценка',
            dataIndex: 'estimate'
        }, {
            header: 'Автор',
            dataIndex: 'author'
        }]
    });

    Ext.create('Ext.panel.Panel', {
        renderTo: 'desktop',
        width: mainWidth,
        height: 450,
        border: false,
        bodyStyle: 'background:transparent;',
        layout: 'border',
        items: [srintsAccordionPanel, backlogGridPanel]
    });
});