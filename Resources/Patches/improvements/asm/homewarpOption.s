.thumb

mov	r0, #0
mov	r8, r0

mov	r0, #1
mov	r9, r0

@ set new quickwarp point
ldr	r0, =#0x2002AC8
mov	r1, #0x22
strb	r1, [r0, #0x00]
mov	r1, #0x15
strb	r1, [r0, #0x01]
mov	r1, #0x04
strb	r1, [r0, #0x02]
mov	r1, #0x00
strb	r1, [r0, #0x03]
ldr	r1, =#-1
str	r1, [r0, #0x04] @ set x and y to max

@ heal link to avoid the noise after the warp
ldr	r0, =#0x2002AC0 + 0x2A
ldrb	r1, [r0, #0x01]
strb	r1, [r0, #0x00]
ldr	r0, =#0x200AF00
lsr	r1, #1
strb	r1, [r0, #0x03]

@ reload
mov	r0, #2
ldr	r3, =#0x80A690C
mov	lr, r3
.short	0xF800
mov	r0, #0x6A
ldr	r3, =#0x80A2A80
mov	lr, r3
.short	0xF800
mov	r0, #5
mov	r1, #8
ldr	r3, =#0x804FC90
mov	lr, r3
.short	0xF800
mov	r0, #2
ldr	r3, =#0x8055B8C
mov	lr, r3
.short	0xF800

@ count it
ldr	r0, =#0x203FE00
ldrh	r1, [r0,#18]
ldr	r2, =#0xFFFF
cmp	r1, r2
beq	end
add	r1, #1
strh	r1, [r0,#18]
ldr	r2, =#0x2000004
ldrb	r2, [r2]
ldr	r1, =#0x500
mul	r2, r1
ldr	r3, =#0xE001400
add	r2, r3
ldrb	r1,[r0,#19]
ldrb	r0,[r0,#18]
strb	r0,[r2,#18]
strb	r1,[r2,#19]

end:
mov	r0, r8
strh	r0, [r6]
strh	r4, [r5]
mov	r1, r9
ldr	r0, =#0x8055E26
mov	lr, r0
.short	0xF800
