using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Movement2D : MonoBehaviour
{

    const float skinwidth = 0.015f;
    BoxCollider2D col;
    RaycastOrigins rays;
    public CollisionInfo collisions;

    public int horizontalRays = 2, verticalRays = 2;
    public LayerMask layerCollision;

    float horizontalRaySpacing, verticalRaySpacing;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(skinwidth * -2);

        horizontalRays = Mathf.Clamp(horizontalRays, 2, int.MaxValue);
        verticalRays = Mathf.Clamp(verticalRays, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRays - 1);
        verticalRaySpacing = bounds.size.x / (verticalRays - 1);

    }

    public void Move(Vector3 velocity)
    {
        collisions.Reset();
        UpdateRaycasts();
        //if (velocity.x != 0)
            HorizontalCollisions(ref velocity);
        //if (velocity.y != 0)
            VerticalCollisions(ref velocity);

        transform.Translate(velocity);
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinwidth;

        for(int i = 0; i < verticalRays; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? rays.bottomleft : rays.topleft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, layerCollision);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {

                velocity.y = (hit.distance - skinwidth) * directionY;
                rayLength = hit.distance;
                
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinwidth;

        for (int i = 0; i < horizontalRays; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? rays.bottomleft : rays.bottomright;
            rayOrigin += Vector2.up * horizontalRaySpacing * i;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, layerCollision);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinwidth) * directionX;
                rayLength = hit.distance;
                
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void UpdateRaycasts()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(skinwidth * -2);

        rays.bottomleft = new Vector2(bounds.min.x, bounds.min.y);
        rays.bottomright = new Vector2(bounds.max.x, bounds.min.y);
        rays.topleft = new Vector2(bounds.min.x, bounds.max.y);
        rays.topright = new Vector2(bounds.max.x, bounds.max.y);
    }

    struct RaycastOrigins
    {
        public Vector2 topleft, topright;
        public Vector2 bottomleft, bottomright;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

}
