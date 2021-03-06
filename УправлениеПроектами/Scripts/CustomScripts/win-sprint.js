function GetSprintWindow(params, title, changeCallback) {

    // окно спринта
    var win = new Ext.Window({
        title: title,
        closable: true,
        resizable: true,
        height: 400,
        width: 1000,
        layout: 'fit',
        buttons: [{
            xtype: 'button',
            text: 'Перейти на страницу спринта',
            handler: function () {
                location.href = '/Sprints/Desktop/' + params.sprintId;
            }
        }],
        items: [
            {
                xtype: 'panel',
                border: false,
                layout: 'fit',
                items: [GetUserStoriesGrid(params.sprintId, changeCallback)]
            }]
    });

    return win;
}