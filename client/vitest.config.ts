import { defineConfig } from "vitest/config";
import { fileURLToPath, URL } from "node:url";

export default defineConfig({
	resolve: {
		alias: {
			"@": fileURLToPath(new URL("./src", import.meta.url)),
		},
	},
	test: {
		coverage: {
			provider: "istanbul",
			reporter: [
				["cobertura", {}],
				["lcov", {}],
				["html", {}],
				["text", {}],
			],
			reportsDirectory: "coverage",
		},
	},
});
