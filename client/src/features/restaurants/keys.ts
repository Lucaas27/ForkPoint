export const restaurantsKeys = {
  all: ['restaurants'] as const,
  detail: (id: number) => ['restaurant', id] as const,
  menu: (id: number) => ['menu', id] as const,
}
