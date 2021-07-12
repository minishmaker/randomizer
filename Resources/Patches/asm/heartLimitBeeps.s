.thumb

@ get our current max health
ldr	r2, =#0x2002A40
mov	r0, r2
add	r0, #0xAB
ldrb	r0, [r0]

@ compare it to the cap
ldr	r1, heartLimited
lsl	r1, #3
cmp	r0, r1
blo	vanilla
@ set max hp to the cap
mov	r0, r1

@ check for the 1 and 2 max hearts beeping exceptions
cmp	r0, #2 << 3
beq	two
cmp	r0, #1 << 3
bne	vanilla

one:
mov	r1, #1 << 1
b	end

two:
mov	r1, #2 << 1

end:
ldr	r3, =#0x801713E
mov	lr, r3
.short	0xF800

vanilla:
ldr	r3, =#0x8017130
mov	lr, r3
.short	0xF800

.align
.ltorg
heartLimited:
