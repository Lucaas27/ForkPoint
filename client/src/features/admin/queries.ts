import { useQuery } from "@tanstack/react-query";
import { getUsers } from "@/api/admin";
import { usersKeys } from "./keys";

export function useUsers(page: number = 1, size: number = 10) {
	return useQuery({
		queryKey: usersKeys.allUsers(page, size),
		queryFn: () => getUsers(page, size),
	});
}

export default useUsers;
