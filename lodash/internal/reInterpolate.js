define("lodash/internal/reInterpolate", [], function() {
  /** Used to match template delimiters. */
  var reInterpolate = /<%=([\s\S]+?)%>/g;

  return reInterpolate;
});
