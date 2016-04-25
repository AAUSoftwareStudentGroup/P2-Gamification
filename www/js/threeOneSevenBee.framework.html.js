/* global Bridge */

"use strict";

Bridge.define('ThreeOneSevenBee.Framework.Html.ElementInput', {
    statics: {
        MOUSE_ID: 0
    },
    pointers: null,
    config: {
        events: {
            PointerEnter: null,
            PointerDown: null,
            PointerMove: null,
            PointerUp: null,
            PointerLeave: null
        },
        properties: {
            Element: null
        }
    },
    constructor: function (element) {
        this.pointers = new Bridge.Dictionary$2(Bridge.Int,ThreeOneSevenBee.Framework.Html.Pointer)();
        this.setElement(element);
        this.getElement().onmouseenter = Bridge.fn.bind(this, this.onMouseEnter);
        this.getElement().onmousemove = Bridge.fn.bind(this, this.onMouseMove);
        this.getElement().onmouseleave = Bridge.fn.bind(this, this.onMouseLeave);
        this.getElement().onmousedown = Bridge.fn.bind(this, this.onMouseDown);
        this.getElement().onmouseup = Bridge.fn.bind(this, this.onMouseUp);

        this.getElement().addEventListener("touchstart", Bridge.fn.bind(this, this.onTouchStart));
        this.getElement().addEventListener("touchmove", Bridge.fn.bind(this, this.onTouchMove));
        this.getElement().addEventListener("touchend", Bridge.fn.bind(this, this.onTouchEnd));
    },
    getPointers: function () {
        return this.pointers.getValues();
    },
    onMouseEnter: function (e) {
        var pointer = { };
        if (!this.pointers.tryGetValue(Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, pointer)) {
            pointer.v = new ThreeOneSevenBee.Framework.Html.Pointer("constructor$1", Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).mouse);
        }

        this.pointers.set(Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, pointer.v);

        pointer.v.update(e, this.getElement());

        var pointerEnter = this.PointerEnter;
        if (pointerEnter !== null) {
            pointerEnter(pointer.v);
        }
    },
    onMouseDown: function (e) {
        var pointer = { };
        if (!this.pointers.tryGetValue(Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, pointer)) {
            pointer.v = new ThreeOneSevenBee.Framework.Html.Pointer("constructor$1", Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).mouse);
        }

        this.pointers.set(Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, pointer.v);

        pointer.v.update(e, this.getElement());

        var pointerDown = this.PointerDown;
        if (pointerDown !== null) {
            pointerDown(pointer.v);
        }
    },
    onMouseMove: function (e) {
        var pointer = { };
        if (!this.pointers.tryGetValue(Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, pointer)) {
            pointer.v = new ThreeOneSevenBee.Framework.Html.Pointer("constructor$1", Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).mouse);
        }

        this.pointers.set(Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, pointer.v);

        pointer.v.update(e, this.getElement());

        var pointerMove = this.PointerMove;
        if (pointerMove !== null) {
            pointerMove(pointer.v);
        }


    },
    onMouseUp: function (e) {
        var pointer = { };
        if (!this.pointers.tryGetValue(Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, pointer)) {
            pointer.v = new ThreeOneSevenBee.Framework.Html.Pointer("constructor$1", Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).mouse);
        }
        pointer.v.update(e, this.getElement());

        var pointerUp = this.PointerUp;
        if (pointerUp !== null) {
            pointerUp(pointer.v);
        }

        this.pointers.remove(Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID);
    },
    onMouseLeave: function (e) {
        var pointer = { };
        if (!this.pointers.tryGetValue(Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, pointer)) {
            pointer.v = new ThreeOneSevenBee.Framework.Html.Pointer("constructor$1", Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID, Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).mouse);
        }
        pointer.v.update(e, this.getElement());

        var pointerLeave = this.PointerLeave;
        if (pointerLeave !== null) {
            pointerLeave(pointer.v);
        }

        this.pointers.remove(Bridge.get(ThreeOneSevenBee.Framework.Html.ElementInput).MOUSE_ID);
    },
    onTouchStart: function (e) {
        var $t;
        var touchEvent = (Bridge.as(e, TouchEvent));
        $t = Bridge.getEnumerator(touchEvent.changedTouches);
        while ($t.moveNext()) {
            var touch = $t.getCurrent();
            var pointer = { };
            if (!this.pointers.tryGetValue(touch.identifier, pointer)) {
                pointer.v = new ThreeOneSevenBee.Framework.Html.Pointer("constructor$1", touch.identifier, Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).touch);
            }

            this.pointers.set(touch.identifier, pointer.v);

            pointer.v.update$1(touch, this.getElement());

            var pointerDown = this.PointerDown;
            if (pointerDown !== null) {
                pointerDown(pointer.v);
            }
        }
    },
    onTouchMove: function (e) {
        var $t;
        var touchEvent = (Bridge.as(e, TouchEvent));
        $t = Bridge.getEnumerator(touchEvent.changedTouches);
        while ($t.moveNext()) {
            var touch = $t.getCurrent();
            var pointer = { };
            if (!this.pointers.tryGetValue(touch.identifier, pointer)) {
                pointer.v = new ThreeOneSevenBee.Framework.Html.Pointer("constructor$1", touch.identifier, Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).mouse);
            }

            this.pointers.set(touch.identifier, pointer.v);

            pointer.v.update$1(touch, this.getElement());

            var pointerMove = this.PointerMove;
            if (pointerMove !== null) {
                pointerMove(pointer.v);
            }
        }
    },
    onTouchEnd: function (e) {
        var $t, $t1;
        var touchEvent = (Bridge.as(e, TouchEvent));
        $t = Bridge.getEnumerator(touchEvent.changedTouches);
        while ($t.moveNext()) {
            var touch = $t.getCurrent();
            var pointer = { };
            if (!this.pointers.tryGetValue(touch.identifier, pointer)) {
                this.pointers.set(touch.identifier, ($t1 = new ThreeOneSevenBee.Framework.Html.Pointer("constructor$1", touch.identifier, Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).mouse), pointer.v$t1, $t1));
            }
            pointer.v.update$1(touch, this.getElement());

            var pointerUp = this.PointerUp;
            if (pointerUp !== null) {
                pointerUp(pointer.v);
            }

            this.pointers.remove(touch.identifier);
        }
    }
});

/** @namespace ThreeOneSevenBee.Framework.Html */

/**
 * A unified Touch/Mouse input system.
 *
 * @public
 * @class ThreeOneSevenBee.Framework.Html.Pointer
 */
Bridge.define('ThreeOneSevenBee.Framework.Html.Pointer', {
    config: {
        properties: {
            Id: 0,
            Type: 0,
            /**
             * Returns the X coordinate of the pointer relative to the left edge of the screen.
             *
             * @instance
             * @public
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function getScreenX
             * @return  {number}
             */
            /**
             * Returns the X coordinate of the pointer relative to the left edge of the screen.
             *
             * @instance
             * @private
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function setScreenX
             * @param   {number}    value
             * @return  {void}
             */
            ScreenX: 0,
            /**
             * Returns the Y coordinate of the pointer relative to the top edge of the screen.
             *
             * @instance
             * @public
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function getScreenY
             * @return  {number}
             */
            /**
             * Returns the Y coordinate of the pointer relative to the top edge of the screen.
             *
             * @instance
             * @private
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function setScreenY
             * @param   {number}    value
             * @return  {void}
             */
            ScreenY: 0,
            /**
             * Returns the X coordinate of the pointer relative to the left edge of the browser viewport, not including any scroll offset.
             *
             * @instance
             * @public
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function getClientX
             * @return  {number}
             */
            /**
             * Returns the X coordinate of the pointer relative to the left edge of the browser viewport, not including any scroll offset.
             *
             * @instance
             * @private
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function setClientX
             * @param   {number}    value
             * @return  {void}
             */
            ClientX: 0,
            /**
             * Returns the Y coordinate of the pointer relative to the top edge of the browser viewport, not including any scroll offset.
             *
             * @instance
             * @public
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function getClientY
             * @return  {number}
             */
            /**
             * Returns the Y coordinate of the pointer relative to the top edge of the browser viewport, not including any scroll offset.
             *
             * @instance
             * @private
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function setClientY
             * @param   {number}    value
             * @return  {void}
             */
            ClientY: 0,
            /**
             * Returns the X coordinate of the pointer relative to the left edge of the document. Unlike clientX, this value includes the horizontal scroll offset, if any.
             *
             * @instance
             * @public
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function getPageX
             * @return  {number}
             */
            /**
             * Returns the X coordinate of the pointer relative to the left edge of the document. Unlike clientX, this value includes the horizontal scroll offset, if any.
             *
             * @instance
             * @private
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function setPageX
             * @param   {number}    value
             * @return  {void}
             */
            PageX: 0,
            /**
             * Returns the Y coordinate of the pointer relative to the top of the document. Unlike clientY, this value includes the vertical scroll offset, if any.
             *
             * @instance
             * @public
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function getPageY
             * @return  {number}
             */
            /**
             * Returns the Y coordinate of the pointer relative to the top of the document. Unlike clientY, this value includes the vertical scroll offset, if any.
             *
             * @instance
             * @private
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function setPageY
             * @param   {number}    value
             * @return  {void}
             */
            PageY: 0,
            RelativeX: 0,
            RelativeY: 0,
            /**
             * Returns the {@link } on which the pointer started when it was first placed on the surface, even if the pointer has since moved outside the interactive area of that element or even been removed from the document.
             *
             * @instance
             * @public
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function getTarget
             * @return  {HTMLElement}
             */
            /**
             * Returns the {@link } on which the pointer started when it was first placed on the surface, even if the pointer has since moved outside the interactive area of that element or even been removed from the document.
             *
             * @instance
             * @private
             * @this ThreeOneSevenBee.Framework.Html.Pointer
             * @memberof ThreeOneSevenBee.Framework.Html.Pointer
             * @function setTarget
             * @param   {HTMLElement}    value
             * @return  {void}
             */
            Target: null
        }
    },
    constructor$1: function (id, type) {
        this.setId(id);
        this.setType(type);
    },
    constructor: function (e, relativeTo) {
        ThreeOneSevenBee.Framework.Html.Pointer.prototype.constructor$1.call(this, 0, Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).touch);

        // mouse id is always 0
        this.setScreenX(e.screenX);
        this.setScreenY(e.screenY);
        this.setClientX(e.clientX);
        this.setClientY(e.clientY);
        this.setPageX(e.pageX);
        this.setPageY(e.pageY);
        this.setTarget(e.target);
        this.setRelativeX(e.pageX - relativeTo.offsetLeft);
        this.setRelativeY(e.pageY - relativeTo.offsetTop);
    },
    constructor$2: function (e, relativeTo) {
        ThreeOneSevenBee.Framework.Html.Pointer.prototype.constructor$1.call(this, e.identifier, Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).touch);

        // touch id is browser controlled
        this.setScreenX(e.screenX);
        this.setScreenY(e.screenY);
        this.setClientX(e.clientX);
        this.setClientY(e.clientY);
        this.setPageX(e.pageX);
        this.setPageY(e.pageY);
        this.setTarget(e.target);
        this.setRelativeX(e.pageX - relativeTo.offsetLeft);
        this.setRelativeY(e.pageY - relativeTo.offsetTop);
    },
    update: function (e, relativeTo) {
        if (!(this.getType() === Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).mouse)) {
            throw new Bridge.InvalidOperationException("Tried to update " + this.getType() + " pointer with mouse data.");
        }

        this.setScreenX(e.screenX);
        this.setScreenY(e.screenY);
        this.setClientX(e.clientX);
        this.setClientY(e.clientY);
        this.setPageX(e.pageX);
        this.setPageY(e.pageY);
        this.setTarget(e.target);
        this.setRelativeX(e.pageX - relativeTo.offsetLeft);
        this.setRelativeY(e.pageY - relativeTo.offsetTop);
    },
    update$1: function (e, relativeTo) {
        if (!(this.getType() === Bridge.get(ThreeOneSevenBee.Framework.Html.PointerTypes).touch)) {
            throw new Bridge.InvalidOperationException("Tried to update " + this.getType() + " pointer with touch data.");
        }

        this.setScreenX(e.screenX);
        this.setScreenY(e.screenY);
        this.setClientX(e.clientX);
        this.setClientY(e.clientY);
        this.setPageX(e.pageX);
        this.setPageY(e.pageY);
        this.setTarget(e.target);
        this.setRelativeX(e.pageX - relativeTo.offsetLeft);
        this.setRelativeY(e.pageY - relativeTo.offsetTop);
    }
});

Bridge.define('ThreeOneSevenBee.Framework.Html.PointerTypes', {
    statics: {
        mouse: 0,
        touch: 1
    },
    $enum: true
});



Bridge.init();