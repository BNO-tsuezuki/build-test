/**
 * Build styles
 */
//require('./text.css').toString();

/**
 * @typedef {object} HeaderData
 * @description Tool's input and output data format
 * @property {string} text — Header's content
 * @property {number} level - Header's level from 1 to 6
 */

/**
 * @typedef {object} HeaderConfig
 * @description Tool's config from Editor
 * @property {string} placeholder — Block's placeholder
 * @property {number[]} levels — Heading levels
 * @property {number} defaultLevel — default level
 */

/**
 * Header block for the Editor.js.
 *
 * @author CodeX (team@ifmo.su)
 * @copyright CodeX 2018
 * @license MIT
 * @version 2.0.0
 */
class Text {
  /**
   * Render plugin`s main Element and fill it with saved data
   *
   * @param {{data: HeaderData, config: HeaderConfig, api: object}}
   *   data — previously saved data
   *   config - user config for Tool
   *   api - Editor.js API
   */
  constructor({ data, config, api }) {
    this.api = api;

    /**
     * Styles
     *
     * @type {object}
     */
    this._CSS = {
      block: this.api.styles.block,
      settingsButton: this.api.styles.settingsButton,
      settingsButtonActive: this.api.styles.settingsButtonActive,
      wrapper: 'ce-header',
    };

    /**
     * Tool's settings passed from Editor
     *
     * @type {HeaderConfig}
     * @private
     */
    this._settings = config;

    /**
     * Block's data
     *
     * @type {HeaderData}
     * @private
     */
    this._data = this.normalizeData(data);

    /**
     * List of settings buttons
     *
     * @type {HTMLElement[]}
     */
    this.settingsButtons = [];

    /**
     * Main Block wrapper
     *
     * @type {HTMLElement}
     * @private
     */
    this._element = this.getTag();
  }

  /**
   * Normalize input data
   *
   * @param {HeaderData} data - saved data to process
   * @returns {HeaderData}
   * @private
   */
  normalizeData(data) {
    const newData = {};

    if (typeof data !== 'object') {
      data = {};
    }

    newData.text = data.text || '';
    newData.level = parseInt(data.level) || this.defaultLevel.number;

    return newData;
  }

  /**
   * Return Tool's view
   *
   * @returns {HTMLHeadingElement}
   * @public
   */
  render() {
    return this._element;
  }

  /**
   * Create Block's settings block
   *
   * @returns {HTMLElement}
   */
  renderSettings() {
    const holder = document.createElement('DIV');

    // do not add settings button, when only one level is configured
    if (this.levels.length <= 1) {
      return holder;
    }

    /** Add type selectors */
    this.levels.forEach(level => {
      const selectTypeButton = document.createElement('SPAN');

      selectTypeButton.classList.add(this._CSS.settingsButton);

      /**
       * Highlight current level button
       */
      if (this.currentLevel.number === level.number) {
        selectTypeButton.classList.add(this._CSS.settingsButtonActive);
      }

      /**
       * Add SVG icon
       */
      selectTypeButton.innerHTML = level.svg;

      /**
       * Save level to its button
       */
      selectTypeButton.dataset.level = level.number;

      /**
       * Set up click handler
       */
      selectTypeButton.addEventListener('click', () => {
        this.setLevel(level.number);
      });

      /**
       * Append settings button to holder
       */
      holder.appendChild(selectTypeButton);

      /**
       * Save settings buttons
       */
      this.settingsButtons.push(selectTypeButton);
    });

    return holder;
  }

  /**
   * Callback for Block's settings buttons
   *
   * @param {number} level - level to set
   */
  setLevel(level) {
    this.data = {
      level: level,
      text: this.data.text,
    };

    /**
     * Highlight button by selected level
     */
    this.settingsButtons.forEach(button => {
      button.classList.toggle(this._CSS.settingsButtonActive, parseInt(button.dataset.level) === level);
    });
  }

  /**
   * Method that specified how to merge two Text blocks.
   * Called by Editor.js by backspace at the beginning of the Block
   *
   * @param {HeaderData} data - saved data to merger with current block
   * @public
   */
  merge(data) {
    const newData = {
      text: this.data.text + data.text,
      level: this.data.level,
    };

    this.data = newData;
  }

  /**
   * Validate Text block data:
   * - check for emptiness
   *
   * @param {HeaderData} blockData — data received after saving
   * @returns {boolean} false if saved data is not correct, otherwise true
   * @public
   */
  validate(blockData) {
    return blockData.text.trim() !== '';
  }

  /**
   * Extract Tool's data from the view
   *
   * @param {HTMLHeadingElement} toolsContent - Text tools rendered view
   * @returns {HeaderData} - saved data
   * @public
   */
  save(toolsContent) {
    return {
      text: toolsContent.innerHTML,
      level: this.currentLevel.number,
    };
  }

  /**
   * Allow Header to be converted to/from other blocks
   */
  static get conversionConfig() {
    return {
      export: 'text', // use 'text' property for other blocks
      import: 'text', // fill 'text' property from other block's export string
    };
  }

  /**
   * Sanitizer Rules
   */
  static get sanitize() {
    return {
      level: false,
      text: {},
    };
  }

  /**
   * Get current Tools`s data
   *
   * @returns {HeaderData} Current data
   * @private
   */
  get data() {
    this._data.text = this._element.innerHTML;
    this._data.level = this.currentLevel.number;

    return this._data;
  }

  /**
   * Store data in plugin:
   * - at the this._data property
   * - at the HTML
   *
   * @param {HeaderData} data — data to set
   * @private
   */
  set data(data) {
    this._data = this.normalizeData(data);

    /**
     * If level is set and block in DOM
     * then replace it to a new block
     */
    if (data.level !== undefined && this._element.parentNode) {
      /**
       * Create a new tag
       *
       * @type {HTMLHeadingElement}
       */
      const newHeader = this.getTag();

      /**
       * Save Block's content
       */
      newHeader.innerHTML = this._element.innerHTML;

      /**
       * Replace blocks
       */
      this._element.parentNode.replaceChild(newHeader, this._element);

      /**
       * Save new block to private variable
       *
       * @type {HTMLHeadingElement}
       * @private
       */
      this._element = newHeader;
    }

    /**
     * If data.text was passed then update block's content
     */
    if (data.text !== undefined) {
      this._element.innerHTML = this._data.text || '';
    }
  }

  /**
   * Get tag for target level
   * By default returns second-leveled header
   *
   * @returns {HTMLElement}
   */
  getTag() {
    /**
     * Create element for current Block's level
     */
    const tag = document.createElement(this.currentLevel.tag);

    /**
     * Add text to block
     */
    tag.innerHTML = this._data.text || '';

    /**
     * Add styles class
     */
    tag.classList.add(this._CSS.wrapper);

    /**
     * Make tag editable
     */
    tag.contentEditable = 'true';

    /**
     * Add Placeholder
     */
    tag.dataset.placeholder = this.api.i18n.t(this._settings.placeholder || '');

    return tag;
  }

  /**
   * Get current level
   *
   * @returns {level}
   */
  get currentLevel() {
    let level = this.levels.find(levelItem => levelItem.number === this._data.level);

    if (!level) {
      level = this.defaultLevel;
    }

    return level;
  }

  /**
   * Return default level
   *
   * @returns {level}
   */
  get defaultLevel() {
    /**
     * User can specify own default level value
     */
    if (this._settings.defaultLevel) {
      const userSpecified = this.levels.find(levelItem => {
        return levelItem.number === this._settings.defaultLevel;
      });

      if (userSpecified) {
        return userSpecified;
      } else {
        console.warn('(ง\'̀-\'́)ง Heading Tool: the default level specified was not found in available levels');
      }
    }

    /**
     * With no additional options, there will be H2 by default
     *
     * @type {level}
     */
    return this.levels[3];
  }

  /**
   * @typedef {object} level
   * @property {number} number - level number
   * @property {string} tag - tag corresponds with level number
   * @property {string} svg - icon
   */

  /**
   * Available header levels
   *
   * @returns {level[]}
   */
  get levels() {
    const availableLevels = [
      {
        number: 1,
        tag: 'H1',
        svg: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="14" viewBox="0 0 16 14"><text x="0" y="14" font-family="Verdana" font-size="12" font-weight="bold">T1</text></svg>',
      },
      {
        number: 2,
        tag: 'H2',
        svg: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="14" viewBox="0 0 16 14"><text x="0" y="14" font-family="Verdana" font-size="12" font-weight="bold">T2</text></svg>',
      },
      {
        number: 3,
        tag: 'H3',
        svg: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="14" viewBox="0 0 16 14"><text x="0" y="14" font-family="Verdana" font-size="12" font-weight="bold">T3</text></svg>',
      },
      {
        number: 4,
        tag: 'H4',
        svg: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="14" viewBox="0 0 16 14"><text x="0" y="14" font-family="Verdana" font-size="12" font-weight="bold">T4</text></svg>',
      },
      {
        number: 5,
        tag: 'H5',
        svg: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="14" viewBox="0 0 16 14"><text x="0" y="14" font-family="Verdana" font-size="12" font-weight="bold">T5</text></svg>',
      },
      {
        number: 6,
        tag: 'H6',
        svg: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="14" viewBox="0 0 16 14"><text x="0" y="14" font-family="Verdana" font-size="12" font-weight="bold">T6</text></svg>',
      },
    ];

    return this._settings.levels ? availableLevels.filter(
      l => this._settings.levels.includes(l.number)
    ) : availableLevels;
  }

  /**
   * Handle H1-H6 tags on paste to substitute it with header Tool
   *
   * @param {PasteEvent} event - event with pasted content
   */
  onPaste(event) {
    const content = event.detail.data;

    /**
     * Define default level value
     *
     * @type {number}
     */
    let level = this.defaultLevel.number;

    switch (content.tagName) {
      case 'H1':
        level = 1;
        break;
      case 'H2':
        level = 2;
        break;
      case 'H3':
        level = 3;
        break;
      case 'H4':
        level = 4;
        break;
      case 'H5':
        level = 5;
        break;
      case 'H6':
        level = 6;
        break;
    }

    if (this._settings.levels) {
      // Fallback to nearest level when specified not available
      level = this._settings.levels.reduce((prevLevel, currLevel) => {
        return Math.abs(currLevel - level) < Math.abs(prevLevel - level) ? currLevel : prevLevel;
      });
    }

    this.data = {
      level,
      text: content.innerHTML,
    };
  }

  /**
   * Used by Editor.js paste handling API.
   * Provides configuration to handle H1-H6 tags.
   *
   * @returns {{handler: (function(HTMLElement): {text: string}), tags: string[]}}
   */
  static get pasteConfig() {
    return {
      tags: ['H1', 'H2', 'H3', 'H4', 'H5', 'H6'],
    };
  }

  /**
   * Get Tool toolbox settings
   * icon - Tool icon's SVG
   * title - title to show in toolbox
   *
   * @returns {{icon: string, title: string}}
   */
  static get toolbox() {
    return {
        icon: '<svg xmlns="http://www.w3.org/2000/svg" width="26" height="16" viewBox="0 0 16 14"><text x="0" y="12" font-family="Verdana" font-size="15" font-weight="bold">T</text></svg>',
      title: 'TEXT',
    };
  }
}

//module.exports = Header;
