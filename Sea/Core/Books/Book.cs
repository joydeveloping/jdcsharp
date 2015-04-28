﻿// Author: Alexey Rybakov

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Sea.Core.Authors;
using Sea.Core.Publishers;

namespace Sea.Core.Books
{
    /// <summary>
    /// Book (printed material item).
    /// </summary>
    public class Book : IComparable<Book>, ICloneable
    {
        /// <summary>
        /// Full name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of book.
        /// </summary>
        public BookType Type { get; set; }

        /// <summary>
        /// Source of article.
        /// (name of magazine or science conference).
        /// </summary>
        public string ArticleSource { get; set; }

        /// <summary>
        /// Edition number.
        /// If 0 - no edition information.
        /// </summary>
        public int Edition { get; set; }

        /// <summary>
        /// Year of publishing.
        /// If 0 - no year information.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// List of authors.
        /// </summary>
        public AuthorsList Authors { get; set; }

        /// <summary>
        /// List of publishers.
        /// </summary>
        public PublishersList Publishers { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="type">type</param>
        /// <param name="article_source">source of article</param>
        /// <param name="edition">edition number</param>
        /// <param name="year">year</param>
        public Book(string name, BookType type, string article_source,
                    int edition, int year)
        {
            Name = name;
            Type = type;
            ArticleSource = article_source;
            Edition = edition;
            Year = year;
            Authors = new AuthorsList();
            Publishers = new PublishersList();
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Book()
            : this("", BookType.Book, "", 1, 0)
        {
        }

        /// <summary>
        /// Compare to another book.
        /// </summary>
        /// <param name="book">book to compare</param>
        /// <returns>1 - if greater, -1 - if less, 0 - if equal</returns>
        public int CompareTo(Book book)
        {
            if (book == null)
            {
                return 1;
            }

            int compare_name = Name.CompareTo(book.Name);

            if (compare_name > 0)
            {
                return 1;
            }
            else if (compare_name < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Clone book.
        /// </summary>
        /// <returns>clone</returns>
        public object Clone()
        {
            Book book = new Book();

            book.Name = Name;
            book.ArticleSource = ArticleSource;
            book.Edition = Edition;
            book.Year = Year;
            book.Authors = Authors.Clone() as AuthorsList;
            book.Publishers = Publishers.Clone() as PublishersList;

            return book;
        }

        /// <summary>
        /// Construct full name of book.
        /// </summary>
        /// <param name="style">style of book printing</param>
        /// <returns>full name</returns>
        public string FullName(BookFullNamePrintStyle style)
        {
            switch (style)
            {
                case BookFullNamePrintStyle.Wide:

                    string full_name = "";

                    // First of all print authors.
                    if (Authors.Count > 0)
                    {
                        full_name = Authors[0].Name(AuthorNamePrintStyle.LastF) + " - ";
                    }

                    // Add name.
                    full_name = full_name + Name;

                    // Year.
                    if (Year > 0)
                    {
                        full_name += String.Format(" ({0})", Year);
                    }

                    return full_name;

                default:
                    Debug.Assert(false, "Unknown book full name print style.");
                    return "";
            }
        }
    }
}
