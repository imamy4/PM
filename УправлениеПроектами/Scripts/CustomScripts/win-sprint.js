function GetSprintWindow(sprintId, title) {

    // окно спринта
    var win = new Ext.Window({
        title: title,
        closable: true,
        resizable: true,
        height: 400,
        width: 700,
        layout: 'fit',
        buttons: [{
            xtype: 'button',
            text: 'Перейти на страницу спринта',
            handler: function () {
                location.href = '/Sprints/Desktop/' + sprintId;
            }
        }],
        items: [
            {
                xtype: 'panel',
                layout: 'fit',
                items: [GetUserStoriesGrid(sprintId)]
            }]
    });

    return win;
}