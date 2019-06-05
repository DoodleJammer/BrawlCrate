﻿using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Modeling
{
    public abstract class SaveState
    {
        public bool _isUndo = true;
    }

    public class CollisionState : SaveState
    {
        public List<CollisionLink> _collisionLinks;

        public CollisionNode _collisionNode;
        public CollisionObject _collisionObject;
        public CollisionPlane _collisionPlane;
        public bool _create;
        public bool _delete;
        public List<Vector2> _linkVectors;
        public bool _merge;
        public bool _split;
    }

    public class VertexState : SaveState
    {
        public int _animFrame;
        public CHR0Node _chr0;
        public IModel _targetModel;
        public List<Vertex3> _vertices = null;
        public List<Vector3> _weightedPositions = null;
    }

    public class BoneState : SaveState
    {
        public CHR0Node _animation;
        public IBoneNode[] _bones;
        public int _frameIndex = 0;
        public FrameState[] _frameStates;
        public IModel _targetModel;
        public bool _updateBindState; //This will update the actual mesh when the bone is moved
        public bool _updateBoneOnly; //This means the bones won't affect the mesh when moved
    }
}