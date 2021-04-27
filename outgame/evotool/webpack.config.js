const path = require('path');

module.exports = {

    mode: 'development',

	entry: { 'evotool': './Client/components/_App.tsx' },

	resolve: {
		extensions: ['.js', '.jsx', '.ts', '.tsx'],
		modules: [
			path.resolve(__dirname, "Client"),
			"node_modules"
		],
	},

	output: {
		path: path.join(__dirname, './wwwroot/js'),
		filename: '[name].js',
		publicPath: 'js/'
	},

	module: {
		rules: [
			{ test: /\.tsx?$/, use: 'ts-loader' }
		]
	},

	devtool: 'inline-source-map'
};
