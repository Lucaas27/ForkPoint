import { describe, it, expect, vi, beforeEach } from "vitest";

// Mock the axios instance
vi.mock("./client", () => ({
	api: {
		post: vi.fn(() => Promise.resolve({ data: {} })),
		delete: vi.fn(() => Promise.resolve({ data: {} })),
	},
}));

import { api } from "./client";

beforeEach(() => {
	vi.clearAllMocks();
});

describe("admin API", () => {
	it("assignRole posts to roles endpoint", async () => {
		const { assignRole } = await import("./admin");
		const payload = { userId: 1, role: "Owner" };
		await assignRole(payload);
		expect(api.post).toHaveBeenCalledWith("/api/admin/users/roles", payload);
	});

	it("removeRole deletes with data payload", async () => {
		const { removeRole } = await import("./admin");
		const payload = { userId: 1, role: "Owner" };
		await removeRole(payload);
		expect(api.delete).toHaveBeenCalledWith("/api/admin/users/roles", {
			data: payload,
		});
	});
});
