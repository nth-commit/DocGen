import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { MatCheckboxChange } from '@angular/material';

import { Document, Template } from '../../../../../_core';

@Component({
  selector: 'app-generator-bulk-documents-table',
  templateUrl: './documents-table.component.html',
  styleUrls: ['./documents-table.component.scss']
})
export class DocumentsTableComponent implements OnInit, OnChanges {
  @Input() template: Template;
  @Input() completedDocuments: Document[];
  @Input() draftDocuments: Document[];
  @Input() selectedDocuments: Document[];
  @Output() documentClicked = new EventEmitter<Document>();
  @Output() documentDeleteClicked = new EventEmitter<Document>();
  @Output() documentCreateFromClicked = new EventEmitter<Document>();
  @Output() documentDownloadClicked = new EventEmitter<Document>();
  @Output() selectedDocumentsUpdated = new EventEmitter<Document[]>();

  displayedColumns = ['selected', 'title', 'values', 'creationTime', 'options'];
  isSelectedByDocumentId: { [key: string]: boolean };

  constructor() { }

  get documents(): Document[] {
    return [
      ...this.completedDocuments,
      ...this.draftDocuments
    ];
  }

  get documentSelectedCount(): number {
    return Object.keys(this.isSelectedByDocumentId)
      .filter(k => this.isSelectedByDocumentId[k])
      .length;
  }

  get documentSelectedMaxCount(): number {
    return this.documents.length;
  }

  ngOnInit() {
    if (!this.selectedDocuments) {
      this.selectedDocuments = [];
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    const updateIsSelectedByDocumentId =
      !this.selectedDocuments ||
      changes.selectedDocuments ||
      changes.completedDocuments ||
      changes.draftDocuments;

    if (!this.selectedDocuments) {
      this.selectedDocuments = [];
    }

    if (updateIsSelectedByDocumentId) {
      const selectedDocumentsById = Map.fromArray(
        this.selectedDocuments,
        d => d.id,
        d => d);

      this.isSelectedByDocumentId = {};
      this.documents.forEach(d => {
        this.isSelectedByDocumentId[d.id] = selectedDocumentsById.has(d.id);
      });
    }
  }

  openDocumentWizard(document: Document) {
    this.documentClicked.emit(document);
  }

  deleteDocument(document: Document) {
    this.documentDeleteClicked.emit(document);
  }

  createFromDocument(document: Document) {
    this.documentCreateFromClicked.emit(document);
  }

  downloadDocument(document: Document) {
    this.documentDownloadClicked.emit(document);
  }

  getFormattedValues(document: Document): string {
    return Object.keys(document.values)
      .filter(k => document.values[k] !== null && document.values[k] !== undefined)
      .map(k => `${k}: ${document.values[k]}`)
      .join(', ');
  }

  onChange(change: MatCheckboxChange) {
    this.documents.forEach(d => this.isSelectedByDocumentId[d.id] = change.checked);
    this.updateSelectedDocuments();
  }

  onDocumentChange(document: Document, change: MatCheckboxChange) {
    this.isSelectedByDocumentId[document.id] = change.checked;
    this.updateSelectedDocuments();
  }

  updateSelectedDocuments() {
    this.selectedDocuments = this.documents.filter(d => this.isSelectedByDocumentId[d.id]);
    this.selectedDocumentsUpdated.emit(this.selectedDocuments);
  }

}
