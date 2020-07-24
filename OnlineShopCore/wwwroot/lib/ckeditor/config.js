/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.on('dialogDefinition', function (ev) {

	var dialogName = ev.data.name,
		dialogDefinition = ev.data.definition;

	if (dialogName == 'image') {
		var onOk = dialogDefinition.onOk;

		dialogDefinition.onOk = function (e) {
			var width = this.getContentElement('info', 'txtWidth');
			width.setValue('750');//Set Default Width

			var height = this.getContentElement('info', 'txtHeight');
			height.setValue('452');//Set Default height

			onOk && onOk.apply(this, e);
		};
	}
});

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here.
	// For complete reference see:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config

	// The toolbar groups arrangement, optimized for two toolbar rows.
	config.toolbarGroups = [
		{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
		{ name: 'editing',     groups: [ 'find', 'selection', 'spellchecker' ] },
		{ name: 'links' },
		{ name: 'insert' },
		{ name: 'forms' },
		{ name: 'tools' },
		{ name: 'document',	   groups: [ 'mode', 'document', 'doctools' ] },
		{ name: 'others' },
		'/',
		{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
		{ name: 'paragraph',   groups: [ 'list', 'indent', 'blocks', 'align', 'bidi' ] },
		{ name: 'styles' },
		{ name: 'colors' },
		{ name: 'about' }
	];

	// Remove some buttons provided by the standard plugins, which are
	// not needed in the Standard(s) toolbar.
	config.removeButtons = 'Underline,Subscript,Superscript';
    config.extraPlugins = 'colorbutton';
	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;pre';

	// Simplify the dialog windows.
    config.removeDialogTabs = 'image:advanced;link:advanced';
    config.filebrowserImageUploadUrl = "/Admin/Upload/UploadImageForCKEditor";
};

CKEDITOR.on('dialogDefinition', function (ev) {
	// Take the dialog name and its definition from the event data.
	var dialogName = ev.data.name;
	var dialogDefinition = ev.data.definition;

	if (dialogName == 'image2') {

		ev.data.definition.dialog.on('show', function () {
			//debugger;
			var widget = ev.data.definition.dialog.widget;
			// To prevent overwriting saved alignment
			if (widget.data['src'].length == 0)
				widget.data['align'] = 'center';

		});

	}
});