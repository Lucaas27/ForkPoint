import { describe, it, expect, vi, beforeEach } from "vitest";

// Mock the axios instance
vi.mock("./client", () => ({
	api: {
		get: vi.fn(() => Promise.resolve({ data: {} })),
		post: vi.fn(() => Promise.resolve({ data: {} })),
		delete: vi.fn(() => Promise.resolve({ data: {} })),
	},
}));

import { api } from "./client";

beforeEach(() => {
	vi.clearAllMocks();
});

describe("menuItems API", () => {
	it("getMenuItems calls the restaurant menu endpoint", async () => {
		const { getMenuItems } = await import("./menuItems");
		await getMenuItems(3);
		expect(api.get).toHaveBeenCalledWith("/api/restaurant/3/menu-items");
	});

	it("getMenuItemById calls the specific menu item endpoint", async () => {
		const { getMenuItemById } = await import("./menuItems");
		await getMenuItemById(3, 12);
		expect(api.get).toHaveBeenCalledWith("/api/restaurant/3/menu-items/12");
	});

	it("createMenuItem posts to the create endpoint", async () => {
		const { createMenuItem } = await import("./menuItems");
		const payload = { name: "Pasta" };
		await createMenuItem(5, payload);
		expect(api.post).toHaveBeenCalledWith(
			"/api/restaurant/5/menu-items/create",
			payload,
		);
	});

	it("deleteMenuItem calls delete on the item endpoint", async () => {
		const { deleteMenuItem } = await import("./menuItems");
		await deleteMenuItem(5, 8);
		expect(api.delete).toHaveBeenCalledWith("/api/restaurant/5/menu-items/8");
	});

	it("deleteAllMenuItems calls delete on the collection endpoint", async () => {
		const { deleteAllMenuItems } = await import("./menuItems");
		await deleteAllMenuItems(5);
		expect(api.delete).toHaveBeenCalledWith("/api/restaurant/5/menu-items");
	});
});
