import React, { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { FiPower, FiEdit, FiTrash2 } from "react-icons/fi";
import api from "../../services/api";
import logoImage from "../../assets/logo.svg";

import "./styles.css";

export default function Books() {
  const navigate = useNavigate();

  const [books, setBooks] = useState([]);
  const [page, setPage] = useState(1);

  const username = localStorage.getItem("username");
  const accessToken = localStorage.getItem("accessToken");

  const headers = {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  };

  useEffect(() => {
    FetchMoreBooks();
  }, [accessToken]);

  async function FetchMoreBooks() {
    try {
      await api.get(`api/book/v1/asc/4/${page}`, headers).then((response) => {
        setBooks([...books, ...response.data.list]);
        setPage(page + 1);
      });
    } catch (error) {
      alert(error);
    }
  }

  async function DeleteBook(id) {
    try {
      api.delete(`api/book/v1/${id}`, headers).then(() => {
        setBooks(books.filter((book) => book.id !== id));
      });
    } catch (error) {
      alert("Delete failed. Try again");
    }
  }

  async function EditBook(id) {
    try {
      navigate(`/book/new/${id}`);
    } catch (error) {
      alert("Edit book failed. Try again");
    }
  }

  async function Logout() {
    try {
      api.get("api/auth/v1/revoke", headers).then(() => {
        localStorage.removeItem("username");
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        navigate("/");
      });
    } catch (error) {
      alert("Logout failed. Try again");
    }
  }

  return (
    <div className="book-container">
      <header>
        <img src={logoImage} alt="Erudio" />
        <span>
          {" "}
          Wellcome, <strong>{username.toUpperCase()}!</strong>
        </span>
        <Link className="button" to="/book/new/0">
          Add new Book
        </Link>
        <button onClick={Logout} type="button">
          <FiPower size={18} color="#251fc5" />
        </button>
      </header>

      <h1>Registered Books</h1>

      <ul>
        {books.map((book) => (
          <li key={book.id}>
            <strong>Title:</strong>
            <p>{book.title}</p>

            <strong>Author:</strong>
            <p>{book.author}</p>

            <strong>Price:</strong>
            <p>
              {Intl.NumberFormat("pt-br", {
                style: "currency",
                currency: "BRL",
              }).format(book.price)}
            </p>

            <strong>Release Data:</strong>
            <p>
              {Intl.DateTimeFormat("pt-br").format(new Date(book.launchDate))}
            </p>

            <button
              onClick={() => {
                EditBook(book.id);
              }}
              type="button"
            >
              <FiEdit size={20} color="#251fc5" />
            </button>
            <button onClick={() => DeleteBook(book.id)} type="button">
              <FiTrash2 size={20} color="#251fc5" />
            </button>
          </li>
        ))}
      </ul>
      <button onClick={FetchMoreBooks} className="button">
        Load More
      </button>
    </div>
  );
}
