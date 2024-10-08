﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

#nullable enable

namespace Stolon
{
    /// <summary>
    /// Provides a way to make a object responsible for dialogue. 
    /// </summary>
    public interface IDialogueProvider
    {
        public string SymbolNotation { get; }
        public string Name { get; }
    }
    public abstract class SLEntity : IDialogueProvider
    {
        public SLEntity(string id, string name, string symbolNotation)
        {
            Id = id;
            Name = name;
            SymbolNotation = symbolNotation;
        }

        public Player GetPlayer()
        {
            return new Player(Name, Computer);
        }

        public abstract SLComputer Computer { get; }
        /// <summary>
        /// The texture thats gets drawn when this entity is selected in the selectEntityMernu.
        /// </summary>
        public abstract Texture2D Splash { get; }
        public virtual string? Description { get; }

        public abstract DialogueInfo GetReaction(PrimitiveReactOption reactOption);

        public string Id { get; private set; }
        public string Name { get; private set; }


        public string SymbolNotation { get; protected set; }

        public enum PrimitiveReactOption
        {
            Afk,
            Distressed,
            Calm,
            GameLost,
            GameWon,
        }
    }
    public readonly struct DialogueInfo
    {
        public string Text { get; }
        public IDialogueProvider Provider { get; }

        public DialogueInfo(IDialogueProvider provider, string text)
        {
            Provider = provider;
            Text = text;
        }


        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return ToString() == (obj == null ? string.Empty : obj).ToString();
        }

        public static bool operator ==(DialogueInfo left, DialogueInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DialogueInfo left, DialogueInfo right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode() * Provider.GetHashCode();
        }
    }
    public abstract class SLComputer
    {
        public SLEntity? Source { get; }

        public SLComputer(SLEntity? source)
        {
            Source = source;
        }

        public abstract void DoMove(Board board);

        public Player GetPlayer(BoardState state)
        {
            Player[] players = state.Players.ToArray();
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].Computer == this)
                {
                    return players[i];
                }
            }
            throw new Exception();
        }
    }
}
