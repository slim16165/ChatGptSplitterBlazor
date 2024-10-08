'use strict';

/**
 * Creates an instance of a {MyInputText},
 * which holds information on the user input.
 * @constructor
 * @this  {MyInputText}
 * @param {String} mode      - the mode of input (i.e. "file" or "text")
 * @param {File}   file      - the file selected by the user
 * @param {String} text      - the input string
 * @param {String} tabPaneId - the id of the tab pane
 */
function MyInputText(mode, file, text, tabPaneId) {
	this.tabPaneId  = tabPaneId;
	this.mode       = mode;
	this.isHTML     = false;
	this.fileName   = (file && file.name);
	this.text       = text;
}

/**
 * Returns the approximate number of pages of the input string.
 * @function
 * @param   {Number} maxCharactersPerPage - the maximum number of characters 
 * 																					per page
 * @returns {Number}                      - the ca. number of pages
 */
MyInputText.prototype.getNumberOfPages = function(maxCharactersPerPage) {
	return (this.text.length / maxCharactersPerPage);
};

/**
 * Resets some fields of the {MyInputText}.
 * @function
 */
MyInputText.prototype.reset = function() {
	this.tabPaneId  = undefined;
	this.mode       = undefined;
	this.fileName   = undefined;
	this.text       = undefined;
};

/**
 * Sets the fields for the file input.
 * @function
 * @param {File}   file      - the file selected by the user
 * @param {String} text      - the file input string
 * @param {String} tabPaneId - the id of the tab pane
 */
MyInputText.prototype.setFileInput = function(file, text, tabPaneId) {
	this.tabPaneId  = tabPaneId;
	this.mode       = 'File';
	this.fileName   = file.name;
	this.text       = text;
};

/**
 * Sets the fields for the text input.
 * @function
 * @param {String} text      - the text input string
 * @param {String} tabPaneId - the id of the tab pane
 */
MyInputText.prototype.setTextInput = function(text, tabPaneId) {
	this.tabPaneId  = tabPaneId;
	this.mode       = 'Text';
	this.fileName   = (this.isHTML) ? 'HTML text input' : 'Plain text input';
	this.text       = text;
};

MyInputText.prototype.setHTMLOption = function(newValue) {
	this.isHTML = newValue;
};

module.exports = MyInputText;