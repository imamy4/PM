function GetCreateUSWindow() {
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
                        'Id',   //числовое значение - номер элемента
                        'Name' //текст
                    ]
                },
                listeners: {
                    change: function (thisCombobox, newValue, oldValue) {
                        categoryCombobox.clearValue();
                        categoryCombobox.store.proxy.url = '/UserStories/GetCategories?projectId=' + thisCombobox.getValue();
                        categoryCombobox.store.load();
                    }
                },
                valueField: 'Id',
                displayField: 'Name',
                queryMode: 'remote',
                typeAhead: true,
                typeAheadDelay: 10
            });

    var categoryCombobox = Ext.create('Ext.form.field.ComboBox',
        {
            id: 'categoryId',
            fieldLabel: 'Категория требования',
            width: 308,
            height: 30,
            margin: '10 10 10 10',
            store: {
                xtype: 'jsonstore',
                autoLoad: true,
                proxy: new Ext.data.HttpProxy({
                    url: '/UserStories/GetCategories?projectId=' + projectCombobox.getValue(),
                    method: 'GET'
                }),
                fields:
                [
                    'Id',   //числовое значение - номер элемента
                    'Name' //текст
                ]
            },
            valueField: 'Id',
            displayField: 'Name',
            queryMode: 'remote',
            typeAhead: true,
            typeAheadDelay: 100
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

    var textFieldDescription = Ext.create('Ext.form.field.TextArea', {
        id: 'description',
        fieldLabel: 'Описание требования',
        name: 'description',
        allowBlank: false,
        height: 60,
        width: 310,
        margin: '10 10 10 10',
        //resizable: true
    });

    var numberFieldEstimate = Ext.create('Ext.form.field.Number', {
        id: 'estimate',
        fieldLabel: 'Первоночальная оценка',
        name: 'estimate',
        height: 20,
        width: 310,
        margin: '10 10 10 10',
    });

    var numberFieldImportance = Ext.create('Ext.form.field.Number', {
        id: 'importance',
        fieldLabel: 'Важность',
        name: 'importance',
        allowBlank: false,
        height: 20,
        width: 310,
        margin: '10 10 10 10',
    });

    // окно создания нового требования
    var win = new Ext.Window({
        title: 'Создание требования',
        closable: false,
        resizable: false,
        height: 400,
        width: 400,
        layout: 'fit',
        items: [{
            xtype: 'form',
            url: '/UserStories/CreateExt',
            layout: 'vbox',
            buttons: [{
                xtype: 'button',
                text: 'Создать требование',
                handler: function () {
                    var form = this.up('form').getForm();
                    if (form.isValid()) {
                        form.submit({
                            params: {
                                projectId: this.up('form').items.get('projectId').getValue(),
                                categoryId: this.up('form').items.get('categoryId').getValue()
                            },
                            success: function (form, action) {
                                Ext.Msg.alert('Сообщение', 'Требование успешно создано.');
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
                textFieldDescription,
                numberFieldEstimate,
                numberFieldImportance,
                categoryCombobox
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