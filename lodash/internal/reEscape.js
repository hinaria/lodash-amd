define("lodash/internal/reEscape", [], function() {
  /** Used to match template delimiters. */
  var reEscape = /<%-([\s\S]+?)%>/g;

  return reEscape;
});
