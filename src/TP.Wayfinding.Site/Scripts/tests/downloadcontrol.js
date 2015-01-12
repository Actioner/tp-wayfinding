// Copyright 2011 Google

/**
 * @license
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/**
 * @author Chris Broadfoot (cbro@google.com)
 */

/**
 * Controls the opacity of an AffineOverlay.
 *
 * @constructor
 * @param {overlaytiler.AffineOverlay} affineOverlay  the overlay to control.
 */
overlaytiler.DownloadControl = function(affineOverlay, canvas) {
    var el = this.el_ = document.createElement('a');
    el.text = 'download tile';
    el.canvas = canvas;
    el.download = 'sosgroso.png';
    el.onclick = this.onClick_.bind(this);

    this.affineOverlay = affineOverlay;
};

/**
 * Called whenever the slider is moved.
 * @private
 */
overlaytiler.DownloadControl.prototype.onClick_ = function () {
    var dt = this.el_.canvas.toDataURL('image/png');
    this.el_.href = dt;
};

/**
 * Returns the Element, suitable for adding to controls on a map.
 * @return {Element}  the Element.
 */
overlaytiler.DownloadControl.prototype.getElement = function() {
  return this.el_;
};
