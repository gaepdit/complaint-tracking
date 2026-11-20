/*!
 * Color mode toggler for Bootstrap's docs (https://getbootstrap.com/)
 * Copyright 2011-2023 The Bootstrap Authors
 * Licensed under the Creative Commons Attribution 3.0 Unported License.
 */

(() => {
    'use strict'

    const getPreferredTheme = () => document.documentElement.dataset.themePreference ?? 'auto'

    const setTheme = theme => {
        if (theme === 'auto') {
            document.documentElement.dataset.bsTheme =
                globalThis.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
        } else {
            document.documentElement.dataset.bsTheme = theme
        }
    }

    setTheme(getPreferredTheme())

    globalThis.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        if (getPreferredTheme() === 'auto') setTheme('auto')
    })
})()
