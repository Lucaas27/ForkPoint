import { describe, it, expect, vi, beforeEach } from "vitest";

// Mock the axios instance
vi.mock("./client", () => ({
	api: {
		get: vi.fn(() => Promise.resolve({ data: {} })),
		post: vi.fn(() => Promise.resolve({ data: {} })),
		patch: vi.fn(() => Promise.resolve({ data: {} })),
		delete: vi.fn(() => Promise.resolve({ data: {} })),
	},
}));

import { getRestaurants } from "./restaurants";
import { api } from "./client";

beforeEach(() => {
	vi.clearAllMocks();
});

describe("getRestaurants", () => {
	it("sends default paging params when none provided", async () => {
		await getRestaurants();

		expect(api.get).toHaveBeenCalledTimes(1);
		expect(api.get).toHaveBeenCalledWith("/api/restaurants", {
			params: { pageNumber: 1, pageSize: 10 },
		});
	});

	it("includes searchBy, searchTerm and categoryFilter when provided", async () => {
		await getRestaurants({
			pageNumber: 2,
			pageSize: 5,
			searchBy: "Name",
			searchTerm: "Padella",
			categoryFilter: "Italian",
		});

		expect(api.get).toHaveBeenCalledTimes(1);
		expect(api.get).toHaveBeenCalledWith("/api/restaurants", {
			params: {
				pageNumber: 2,
				pageSize: 5,
				searchBy: "Name",
				searchTerm: "Padella",
				categoryFilter: "Italian",
			},
		});
	});
});

describe("getRestaurantById", () => {
	it("calls the correct endpoint", async () => {
		await getRestaurants(); // ensure previous calls don't interfere
		const { getRestaurantById } = await import("./restaurants");

		await getRestaurantById(42);
		expect(api.get).toHaveBeenCalledWith("/api/restaurants/42");
	});
});

describe("createRestaurant", () => {
	it("posts payload to create endpoint", async () => {
		const { createRestaurant } = await import("./restaurants");
		const payload = { name: "New Resto" };
		await createRestaurant(payload);
		expect(api.post).toHaveBeenCalledWith("/api/restaurants/create", payload);
	});
});

describe("updateRestaurant", () => {
	it("patches payload to the id endpoint", async () => {
		const { updateRestaurant } = await import("./restaurants");
		const payload = { name: "Updated" };
		await updateRestaurant(7, payload);
		expect(api.patch).toHaveBeenCalledWith("/api/restaurants/7", payload);
	});
});

describe("deleteRestaurant", () => {
	it("calls delete on id endpoint", async () => {
		const { deleteRestaurant } = await import("./restaurants");
		await deleteRestaurant(9);
		expect(api.delete).toHaveBeenCalledWith("/api/restaurants/9");
	});
});
