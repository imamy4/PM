function GetBurndownChart(storeUrl) {

    var burndownChart = Ext.create('Ext.panel.Panel', {
        title: 'Диаграмма сгорания задач',
        layout: 'fit',
        frame: true,
        margin: '5 5 5 5',
        items: {
            xtype: 'chart',
            animate: true,
            legend: {
                position: 'bottom'
            },
            store: {
                xtype: 'jsonstore',
                autoLoad: true,
                proxy: new Ext.data.HttpProxy({
                    url: storeUrl,
                    method: 'GET'
                }),
                fields:
                [
                    'date',
                    'ideal',
                    'real'
                ]
            },
            insetPadding: 30,
            axes: [{
                type: 'Numeric',
                minimum: 0,
                position: 'left',
                fields: ['ideal'],
                title: false,
                grid: true,
                label: {
                    renderer: Ext.util.Format.numberRenderer('0,0')
                }
            }, {
                type: 'Numeric',
                minimum: 0,
                position: 'right',
                fields: ['real'],
                title: false,
                grid: true,
                label: {
                    renderer: Ext.util.Format.numberRenderer('0,0')
                }
            }, {
                type: 'Category',
                position: 'bottom',
                fields: ['date'],
                title: false,
                label: {
                    renderer: function (name) {
                        return new Date(name).toLocaleDateString();
                    }
                }
            }],
            series: [{
                type: 'line',
                title: 'Линия сгорания задач (идеальная)',
                axis: 'left',
                xField: 'date',
                yField: 'ideal',
                tips: {
                    trackMouse: true,
                    width: 120,
                    height: 40,
                    renderer: function (storeItem, item) {
                        this.setTitle(new Date(storeItem.get('date')).toLocaleDateString() + '<br />' + 'Осталось: ' + storeItem.get('hours'));
                    }
                },
                style: {
                    fill: '#38B8BF',
                    stroke: '#38B8BF',
                    'stroke-width': 3
                },
                markerConfig: {
                    type: 'circle',
                    size: 4,
                    radius: 4,
                    'stroke-width': 0,
                    fill: '#38B8BF',
                    stroke: '#38B8BF'
                }
            }, {
                type: 'line',
                axis: 'right',
                title: 'Линия сгорания задач (фактическая)',
                xField: 'date',
                yField: 'real',
                tips: {
                    trackMouse: true,
                    width: 120,
                    height: 40,
                    renderer: function (storeItem, item) {
                        this.setTitle(new Date(storeItem.get('date')).toLocaleDateString() + '<br />' + 'Осталось: ' + storeItem.get('real'));
                    }
                },
                style: {
                    fill: '#B838BF',
                    stroke: '#B838BF',
                    'stroke-width': 3
                },
                markerConfig: {
                    type: 'circle',
                    size: 4,
                    radius: 4,
                    'stroke-width': 0,
                    fill: '#B838BF',
                    stroke: '#B838BF'
                }
            }]
        }
    });

    return burndownChart;
}