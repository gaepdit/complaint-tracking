/// <binding Clean='clean' BeforeBuild='default' />
'use strict'

const { series, parallel, src, dest } = require('gulp')
const rimraf = require('rimraf')

var paths = {
  webRoot: './wwwroot/',
  assetsRoot: './wwwroot/assets/',
  nodeSource: './node_modules/',
  wdsSource: './node_modules/ga-epd-wds/dist/**/*',
  wdsFaviconsSource: './node_modules/ga-epd-wds/dist/epd-favicons/*',
}

// Copy GA-EPD WDS files to www assets folder
function GaWds() {
  return src([paths.wdsSource, '!' + paths.wdsFaviconsSource]).pipe(
    dest(paths.assetsRoot)
  )
}

function RemoveExtraneousFaviconsFolder(cb) {
  rimraf(paths.assetsRoot + 'epd-favicons', cb)
}

// Copy GA-EPD favicon files to www root folder
function CopyGaFavicons() {
  return src(paths.wdsFaviconsSource).pipe(dest(paths.webRoot))
}

// Copy library files to www root folder
function CopyJquery() {
  return src([paths.nodeSource + 'jquery/dist/*']).pipe(
    dest(paths.assetsRoot + 'lib/jquery')
  )
}

function CopyJqueryUi() {
  return src([paths.nodeSource + 'jquery-ui-dist/**/*']).pipe(
    dest(paths.assetsRoot + 'lib/jquery-ui')
  )
}

function CopyJqueryTimepicker() {
  return src([paths.nodeSource + 'jquery-timepicker/*']).pipe(
    dest(paths.assetsRoot + 'lib/jquery-timepicker')
  )
}

function CopyJqueryValidation() {
  return src([paths.nodeSource + 'jquery-validation/dist/**/*']).pipe(
    dest(paths.assetsRoot + 'lib/jquery-validation')
  )
}

function CopyJqueryValidationUnobtrusive() {
  return src([paths.nodeSource + 'jquery-validation-unobtrusive/dist/*']).pipe(
    dest(paths.assetsRoot + 'lib/jquery-validation-unobtrusive')
  )
}

function CopyFancybox() {
  return src([paths.nodeSource + '@fancyapps/fancybox/dist/**/*']).pipe(
    dest(paths.assetsRoot + 'lib/fancybox')
  )
}

// Clean web root
function clean(cb) {
  rimraf(paths.webRoot + '!(static)', cb)
}

// Export tasks

exports.default = parallel(
  series(GaWds, RemoveExtraneousFaviconsFolder),
  CopyGaFavicons,
  CopyJquery,
  CopyJqueryUi,
  CopyJqueryTimepicker,
  CopyJqueryValidation,
  CopyJqueryValidationUnobtrusive,
  CopyFancybox
)
exports.clean = clean
