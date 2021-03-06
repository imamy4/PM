function GetCreateSprintWindow() {
    var projectCombobox = Ext.create('Ext.form.field.ComboBox',
            {
                id: 'projectId',
                fieldLabel: 'Проект',
                width: 308,
                height: 30,
                margin: '10 10 10 10',
                store: {
                    xtype: 'jsonstore',
                    autoLoad: true,
                    proxy: new Ext.data.HttpProxy({
                        url: '/UserStories/GetProjects',
                        method: 'GET'
                    }),
                    fields:
                    [
                        'id',   //числовое значение - номер элемента
                        'name' //текст
                    ]
                },
                valueField: 'id',
                displayField: 'name',
                queryMode: 'remote',
                typeAhead: true,
                typeAheadDelay: 10
            });

    var textFieldName = Ext.create('Ext.form.field.Text', {
        id: 'name',
        fieldLabel: 'Наименование требования',
        name: 'name',
        allowBlank: false,
        height: 20,
        width: 310,
        margin: '10 10 10 10',
    });

    var dateStartField = Ext.create('Ext.form.field.Date', {
        id: 'dateStart',
        fieldLabel: 'Дата начала',
        margin: '10 10 10 10',
        allowBlank: false,
        format: 'd-m-Y',
    });

    var dateFinishField = Ext.create('Ext.form.field.Date', {
        id: 'dateFinish',
        fieldLabel: 'Дата конца',
        format: 'd-m-Y',
        allowBlank: false,
        format: 'd-m-Y',
        margin: '10 10 10 10',
    });

    // окно создания нового спринта
    var win = new Ext.Window({
        title: 'Создание спринта',
        closable: false,
        resizable: false,
        height: 400,
        width: 400,
        layout: 'fit',
        items: [{
            xtype: 'form',
            url: '/Sprints/CreateExt',
            layout: 'vbox',
            buttons: [{
                xtype: 'button',
                text: 'Создать спринт',
                handler: function () {
                    var form = this.up('form').getForm();
                    if (form.isValid()) {
                        form.submit({
                            params: {
                                projectId: this.up('form').items.get('projectId').getValue(),
                                dateStart: this.up('form').items.get('dateStart').getValue(),
                                dateFinish: this.up('form').items.get('dateFinish').getValue()
                            },
                            success: function (form, action) {
                                Ext.Msg.alert('Сообщение', 'Спринт успешно создан.');
                                win.close();
                                if (win.successCallback) {
                                    win.successCallback();
                                }
                            },
                            failure: function (form, action) {
                                Ext.Msg.alert('Произошла ошибка.');
                            }
                        });
                    }
                }
            },
                '->',
                {
                    xtype: 'button',
                    text: 'Отмена',
                    handler: function () {
                        win.close()
                    }
                }],
            items: [projectCombobox,
                textFieldName,
                dateStartField,
                dateFinishField
            ]
        }],
        listeners: {
            beforerender: function (thisWin) {
                if (thisWin.projectId) {
                    var projectCombobox = Ext.getCmp('projectId');
                    projectCombobox.setValue(thisWin.projectId);
                    projectCombobox.setReadOnly(true);
                }
            }
        }
    });

    return win;
}