.thumb
ldr	r3,=#0x3001174
ldrh	r0,[r3,#0x1A] @ X position
ldrh	r1,=#0x07E8
ldrb	r3,[r3] @ facing direction
cmp	r3,#0x02
bne	notFacingRight

add	r0,#0x10
b	notFacingLeft

notFacingRight:
cmp	r3,#0x06
bne	notFacingLeft

sub	r0,#0x10

notFacingLeft:
cmp	r0,r1
blo	left

add	r1,#0x10
cmp	r0,r1
blo	middle

right:
add	r5,#1

middle:
add	r5,#1

left:
ldr	r3,=#0x8069141
bx	r3
