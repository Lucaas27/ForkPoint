export const restaurantsKeys = {
	list: (
		page: number,
		size: number,
		searchBy?: string,
		searchTerm?: string,
		categoryFilter?: string,
	) =>
		[
			"restaurants",
			{ page, size, searchBy, searchTerm, categoryFilter },
		] as const,
	detail: (id: number) => ["restaurant", id] as const,
	menu: (id: number) => ["menu", id] as const,
};
