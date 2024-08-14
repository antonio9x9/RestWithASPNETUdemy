import React, { useState, useEffect } from "react";
import "./styles.css";
import { Link, useNavigate, useParams } from "react-router-dom";

import { FiArrowLeft } from "react-icons/fi";

import api from "../../services/api";
import logoImage from "../../assets/logo.svg";

export default function NewBook() {
  const navigate = useNavigate();

  const accessToken = localStorage.getItem("accessToken");

  const headers = {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  };

  const [id, setId] = useState(null);
  const [title, setTile] = useState("");
  const [author, setAuthor] = useState("");
  const [date, setDate] = useState("");
  const [price, setPrice] = useState(0);

  const { bookId } = useParams();

  useEffect(() => {
    if (bookId == 0) {
      return;
    } else {
      LoadBook();
    }
  }, [bookId]);

  async function LoadBook() {
    try {
      let response = await api.get(`api/book/v1/${bookId}`, headers);

      let adjustedDate = response.data.launchDate.split("T", 10)[0];

      setId(response.data.id);
      setTile(response.data.title);
      setAuthor(response.data.author);
      setPrice(response.data.price);
      setDate(adjustedDate);
    } catch (error) {
      alert("Error recovering book! Try again");
      navigate("/books");
    }
  }

  async function SaveOrUpdate(e) {
    e.preventDefault();

    const data = {
      title: title,
      author: author,
      launchDate: date,
      price: price,
    };

    try {
      if (bookId === "0") {
        await api.post("api/book/v1", data, headers).catch((error) => {
          if (error.response) {
            // The server responded with a status code outside the 2xx range
            console.log("Error response:", error.response);
          } else if (error.request) {
            // The request was made but no response was received
            console.log("Error request:", error.request);
          } else {
            // Something happened in setting up the request that triggered an error
            console.log("Error message:", error.message);
          }
        });
      } else {
        data.id = bookId;
        await api.put("api/book/v1", data, headers);
      }
      alert("Successful!");
      navigate("/books");
    } catch (error) {
      alert("Error while reacording Book! Try Again");
    }
  }

  return (
    <div className="new-book-container">
      <div className="content">
        <section className="form">
          <img src={logoImage} alt="Erudio" />
          <h1>{bookId == 0 ? "Add" : "Edit"} new Book</h1>
          <p>
            Enter the book information and clik on '
            {bookId == 0 ? "Add" : "Update"}'!
          </p>
          <Link className="back-link" to={"/books"}>
            <FiArrowLeft size={16} color="#251fc5" />
            Back to books
          </Link>
        </section>
        <form onSubmit={SaveOrUpdate}>
          <input
            value={title}
            onChange={(e) => setTile(e.target.value)}
            placeholder="Title"
          />
          <input
            value={author}
            onChange={(e) => setAuthor(e.target.value)}
            placeholder="Author"
          />
          <input
            value={date}
            onChange={(e) => setDate(e.target.value)}
            type="date"
          />
          <input
            value={price}
            onChange={(e) => setPrice(e.target.value)}
            placeholder="Price"
          />

          <button className="button" type="submit">
            {bookId == 0 ? "Add" : "Update"}
          </button>
        </form>
      </div>
    </div>
  );
}
