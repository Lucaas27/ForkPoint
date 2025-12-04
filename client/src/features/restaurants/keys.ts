export const restaurantsKeys = {
	all: ["restaurants"] as const,
	list: (page: number, size: number) =>
		["restaurants", { page, size }] as const,
	detail: (id: number) => ["restaurant", id] as const,
	menu: (id: number) => ["menu", id] as const,
};
