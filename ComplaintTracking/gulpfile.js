/// <binding Clean='clean' BeforeBuild='default' />
'use strict'

const { parallel, src, dest } = require('gulp')
const rimraf = require('rimraf')

const paths = {
  webRoot: './wwwroot/',
  libRoot: './wwwroot/assets/lib/',
  nodeSource: './node_modules/',
}

// Copy library files to www root folder
function CopyJquery() {
  return src([paths.nodeSource + 'jquery/dist/*']).pipe(
    dest(paths.libRoot + 'jquery')
  )
}

function CopyJqueryUi() {
  return src([paths.nodeSource + 'jquery-ui-dist/**/*']).pipe(
    dest(paths.libRoot + 'jquery-ui')
  )
}

function CopyJqueryTimepicker() {
  return src([paths.nodeSource + 'jquery-timepicker/*']).pipe(
    dest(paths.libRoot + 'jquery-timepicker')
  )
}

function CopyJqueryValidation() {
  return src([paths.nodeSource + 'jquery-validation/dist/**/*']).pipe(
    dest(paths.libRoot + 'jquery-validation')
  )
}

function CopyJqueryValidationUnobtrusive() {
  return src([paths.nodeSource + 'jquery-validation-unobtrusive/dist/*']).pipe(
    dest(paths.libRoot + 'jquery-validation-unobtrusive')
  )
}

function CopyFancybox() {
  return src([paths.nodeSource + '@fancyapps/fancybox/dist/**/*']).pipe(
    dest(paths.libRoot + 'fancybox')
  )
}

// Clean web root
function clean(cb) {
  rimraf(paths.libRoot, cb)
}

// Export tasks
exports.default = parallel(
  CopyJquery,
  CopyJqueryUi,
  CopyJqueryTimepicker,
  CopyJqueryValidation,
  CopyJqueryValidationUnobtrusive,
  CopyFancybox
)
exports.clean = clean
