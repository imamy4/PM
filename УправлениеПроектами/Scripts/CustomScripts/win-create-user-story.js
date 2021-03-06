function GetCreateUSWindow(successCallback, defaultValues) {

    var submitHandler = function () {
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
                    if (successCallback) {
                        successCallback();
                    }
                },
                failure: function (form, action) {
                    Ext.Msg.alert('Произошла ошибка.');
                }
            });
        }
    };
    var cancelHandler = function () {
        win.close();
    }

    // окно создания нового требования
    var win = new Ext.Window({
        title: 'Создание требования',
        closable: false,
        resizable: false,
        height: 500,
        width: 450,
        layout: 'fit',
        items: [GetCreateUSForm(submitHandler, cancelHandler, '/UserStories/CreateExt', defaultValues)]
    });

    return win;
}

function GetCreateUSForm(submitHandler, cancelHandler, url, defaultValues) {
    var projectCombobox = Ext.create('Ext.form.field.ComboBox', {
        id: 'projectId',
        fieldLabel: 'Проект',
        value: defaultValues && defaultValues.projectId ? defaultValues.projectId : "",
        readOnly: defaultValues && defaultValues.projectId ? true : false,
        width: 385,
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
                'id',
                'name'
            ]
        },
        listeners: {
            change: function (thisCombobox, newValue, oldValue) {
                categoryCombobox.clearValue();
                categoryCombobox.store.proxy.url = '/UserStories/GetCategories?projectId=' + thisCombobox.getValue();
                categoryCombobox.store.load();
            }
        },
        valueField: 'id',
        displayField: 'name',
        queryMode: 'remote',
        typeAhead: true,
        typeAheadDelay: 10
    });

    var categoryCombobox = Ext.create('Ext.form.field.ComboBox', {
        id: 'categoryId',
        fieldLabel: 'Категория требования',
        value: defaultValues && defaultValues.categoryId ? defaultValues.categoryId : "",
        readOnly: defaultValues && defaultValues.categoryId ? true : false,
        width: 385,
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
                'id',   //числовое значение - номер элемента
                'name' //текст
            ]
        },
        valueField: 'id',
        displayField: 'name',
        queryMode: 'remote',
        typeAhead: true,
        typeAheadDelay: 100
    });

    var textFieldName = Ext.create('Ext.form.field.Text', {
        id: 'name',
        fieldLabel: 'Наименование требования',
        value: defaultValues && defaultValues.name ? defaultValues.name : "",
        readOnly: defaultValues && defaultValues.name ? true : false,
        name: 'name',
        allowBlank: false,
        height: 25,
        width: 385,
        margin: '10 10 10 10',
    });

    var textFieldDescription = Ext.create('Ext.form.field.TextArea', {
        id: 'description',
        fieldLabel: 'Описание требования',
        value: defaultValues && defaultValues.description ? defaultValues.description : "",
        readOnly: defaultValues && defaultValues.description ? true : false,
        name: 'description',
        allowBlank: false,
        height: 120,
        width: 385,
        margin: '10 10 10 10',
        //resizable: true
    });

    var numberFieldEstimate = Ext.create('Ext.form.field.Number', {
        id: 'estimate',
        fieldLabel: 'Первоночальная оценка (в часах)',
        value: defaultValues && defaultValues.estimate ? defaultValues.estimate : "",
        readOnly: defaultValues && defaultValues.estimate ? true : false,
        hideTrigger: true,
        name: 'estimate',
        height: 20,
        width: 385,
        margin: '10 10 10 10',
    });

    var numberFieldImportance = Ext.create('Ext.form.field.Number', {
        id: 'importance',
        fieldLabel: 'Важность',
        value: defaultValues && defaultValues.importance ? defaultValues.importance : "",
        readOnly: defaultValues && defaultValues.importance ? true : false,
        hideTrigger: true,
        name: 'importance',
        allowBlank: false,
        height: 20,
        width: 385,
        margin: '10 10 10 10',
    });

    var formParams = {
        url: url,
        layout: 'vbox',
        buttons: [],
        items: [projectCombobox,
            textFieldName,
            textFieldDescription,
            numberFieldEstimate,
            numberFieldImportance,
            categoryCombobox
        ]
    };
    
    if (submitHandler) {
        formParams.buttons.push({
            xtype: 'button',
            text: 'Создать требование',
            handler: submitHandler
        });
    }
    if (cancelHandler) {
        formParams.buttons.push('->');
        formParams.buttons.push({
            xtype: 'button',
            text: 'Отмена',
            handler: cancelHandler
        });
    }
    

    var createUSForm = Ext.create('Ext.form.Panel', formParams);

    return createUSForm;
}