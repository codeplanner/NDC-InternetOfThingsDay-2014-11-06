var Gauge = (function () {
    'use strict';
    
    var ctor = function (selector, props, onChanged) {        
        // init Wijmo controls        
        this.gauge = new wijmo.gauge.LinearGauge(selector);
        this.gauge.value = props.value;
        this.gauge.min = props.min;
        this.gauge.max = props.max;
        this.gauge.format = props.format;

        // set LinearGauge properties
        this.gauge.step = props.step;
        this.gauge.isReadOnly = props.isReadOnly;
        // set LinearGauge properties
        this.gauge.showText = wijmo.gauge.ShowText.Value;

        //By default do not broadcast changes
        this.gauge.cascadeChanges = true;

        // Eventhandler for value changed
        this.gauge.valueChanged.addHandler(onChanged);        
    };    
    return ctor;
})();