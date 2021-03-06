var firstColor;
var secondColor = '#05b944';
var selected = {};
var sum = 0;
var count = 0;
var maxCount = 3;
var operationType = null;

onload = function () {
    var out = document.getElementById("resOperation");

    var dropList = document.getElementById("drop_list");
    if (dropList != null) {
        operationType = "Покупка";
        dropList.onchange = function () {
            var operSpan = document.getElementById("oper_type");
            operSpan.innerHTML = this.value;
            operationType = this.value;
        }
    }

    var subBut = document.getElementById("submitBut");
    subBut.onclick = function () {
        var id;
        var attrValue = out.getAttribute("value");
        var status = parseInt(attrValue.split('|')[0]) + 1;
        attrValue = status + attrValue.slice(attrValue.indexOf("|"));

        for (id in selected) {
            if (selected[id]) {
                attrValue += id + ";";
            }
        }

        if (operationType != null) {
            attrValue += "|" + operationType;
        }
        out.setAttribute("value", attrValue);
    }

    var elements;
    elements = document.getElementsByClassName("seat-free");

    for (var i = 0; i < elements.length; i++) {
        var elem = elements[i];

        firstColor = elem.style.backgroundColor;

        var id = elem.getAttribute('id');
        selected[id] = false;

        elem.onclick = function () {
            this.style.backgroundColor = firstColor;

            var id = this.getAttribute('id');
            if (selected[id]) {
                this.style.backgroundColor = firstColor;
                selected[id] = false;
                deleteSeat(id);
            }
            else {
                if (count == maxCount) {
                    warning();
                }
                else {
                    this.style.backgroundColor = secondColor;
                    selected[id] = true;
                    addSeat(id);
                }
            }
        };
    }       
};

function warning() {
    var panel = document.getElementById('left_result');

    if (panel.innerHTML.search(/Нельзя/) == -1) {
        panel.innerHTML = panel.innerHTML + '<br>' + '<b>Нельзя выбрать больше ' + count + ' мест</b>';
    }
}

function addSeat(id) {
    var panel = document.getElementById('left_result');
    var seat = document.getElementById(id);

    sum -= -getPrice(seat.getAttribute('data'));
    count++;

    panel.innerHTML = 'Выбрано билетов: ' + count + '<br>На сумму: ' + sum;
}


function deleteSeat(id) {
    var panel = document.getElementById('left_result');
    var seat = document.getElementById(id);

    sum -= 0 + getPrice(seat.getAttribute('data'));
    count--;
    
    panel.innerHTML = 'Выбрано билетов: ' + count + '<br>На сумму: ' + sum;
}


function getPrice(str) {
    str = str.replace(',', '.');
    var pattern = /price=\d+.\d*/gi;
    
    var matches = str.match(pattern);

    return matches[0].split('=')[1];
}
