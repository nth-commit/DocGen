Ideas:
- Possibly generate document with angular, or use a subset of angular (do a transformation from out subset to angular template)
- Subset could support only a few different elements (<page>, <section>, <b>, <i>), and attributes (if => ngIf), and logic (== => ===)
- Store template as text/html.docgen and return as text/html.angular (content types)
- When making templates, PDF pages are generated independently, and provide warnings when page content exceeds page size