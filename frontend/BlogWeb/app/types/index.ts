export interface ApiResponse<T> {
  data: T;
  message?: string;
  error?: string;
  success: boolean;
}

export interface AuthResponse {
  token: string;
}

export interface Category {
  id: number;
  name: string;
}

export enum Status {
  Draft,
  Published,
  Archived,
}

export interface Post {
  id: number;
  title: string;
  slug: string;
  content: string;
  excerpt: string;
  featuredImageUrl: string;
  tags: string;
  categoryId: number;
  authorId: number;
  createdAt: string;
  updatedAt: string;
  status: Status;
}

export interface UserInfo {
  id: number;
  name: string;
  email?: string;
}

export interface PaginationList<T> {
  items: T[];
  totalCount: number;
}

export interface PaginationQuery {
  pageSize: number;
  pageNumber: number;
}
export interface ListPostResponse extends PaginationList<Post> {}

export interface ListPostQuery extends PaginationQuery {
  titleContains?: string;
  categoryId?: number;
  staus?: Status;
}
